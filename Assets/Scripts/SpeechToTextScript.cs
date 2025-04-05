using System;
using System.Collections;
using System.IO;
using HuggingFace.API;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechRecognitionTest : MonoBehaviour {
    [SerializeField] private Button startButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private TextMeshProUGUI text;

    private AudioClip clip;
    private bool recording;
    private Coroutine recordingRoutine;

    private void Start() {
        startButton.onClick.AddListener(() => StartRecording());
        stopButton.onClick.AddListener(() => StopRecording());

        stopButton.interactable = false;
    }

    private void StartRecording() {
        recording = true;
        text.text = "Listening...";
        text.color = Color.white;

        startButton.interactable = false;
        stopButton.interactable = true;

        recordingRoutine = StartCoroutine(RecordAndSendLoop());
    }

    private void StopRecording() {
        recording = false;
        text.text = "Stopped";
        text.color = Color.gray;

        startButton.interactable = true;
        stopButton.interactable = false;

        if (recordingRoutine != null)
            StopCoroutine(recordingRoutine);
    }

    private IEnumerator RecordAndSendLoop() {
        while (recording) {
            clip = Microphone.Start(null, false, 4, 44100);
            yield return new WaitForSeconds(4f);

            Microphone.End(null);

            float[] samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            byte[] bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);

            SendRecording(bytes);

            yield return new WaitForSeconds(0.5f); 
        }
    }

    private void SendRecording(byte[] audioBytes) {
        text.color = Color.yellow;
        HuggingFaceAPI.AutomaticSpeechRecognition(audioBytes, response => {
            text.color = Color.green;
            text.text = response;
        }, error => {
            text.color = Color.red;
            text.text = error;
        });
    }

    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels) {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2)) {
            using (var writer = new BinaryWriter(memoryStream)) {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples) {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }
}
