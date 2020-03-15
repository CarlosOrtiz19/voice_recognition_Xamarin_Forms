using AVFoundation;
using Foundation;
using Speech;
using System;
using System.Diagnostics;
using TP2V3;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextIOS))]
namespace TP2V3
{
    public class SpeechToTextIOS : ISpeechToText
    {
        // Speech
        private AVAudioEngine m_audioEngine;  //propres de IOS
        private SFSpeechRecognizer m_speechRecognizer;
        private SFSpeechAudioBufferRecognitionRequest m_liveSpeechRequest;
        private SFSpeechRecognitionTask m_recognitionTask;

        public event EventHandler<string> OnSpeechResult;
        private string language;

        public SpeechToTextIOS()
        {
            m_audioEngine = new AVAudioEngine();
        }


        // ------------------------------------- FUNCTIONS ------------------------------------- //


        public void StartASR()
        {
            m_speechRecognizer = new SFSpeechRecognizer(new NSLocale(this.language));
            Debug.WriteLine(this.language + " desde ios");
            StartRecording();
        }


        public void StopASR()
        {
            if (m_audioEngine.Running)
            {
                m_audioEngine.Stop();
                m_liveSpeechRequest?.EndAudio();
            }
            else
            {
                m_recognitionTask?.Cancel();
            }
            m_audioEngine?.InputNode.RemoveTapOnBus(0);
            m_recognitionTask = null;
            m_liveSpeechRequest = null;
        }

        private void FixAudioOutputVolume()
        {
            var audioSession = AVAudioSession.SharedInstance();
            audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord, AVAudioSessionCategoryOptions.DefaultToSpeaker);
            audioSession.SetMode(AVAudioSession.ModeDefault, out NSError errorMode);
            if (errorMode != null)
            {
                Debug.WriteLine("ERROR: setting audio session mode FAILED");
            }
        }

        private void StartRecording()
        {
            FixAudioOutputVolume();

            var node = m_audioEngine.InputNode;
            var recordingFormat = node.GetBusOutputFormat(0);
            m_liveSpeechRequest = new SFSpeechAudioBufferRecognitionRequest();
            node.InstallTapOnBus(0, 1024, recordingFormat, (AVAudioPcmBuffer buffer, AVAudioTime when) =>
            {
                m_liveSpeechRequest.Append(buffer);
            });

            m_audioEngine.Prepare();
            m_audioEngine.StartAndReturnError(out NSError error);
            if (error != null)
            {
                Debug.WriteLine("ERROR: Recording failed");
                return;
            }

            m_recognitionTask = m_speechRecognizer.GetRecognitionTask(m_liveSpeechRequest, ProcessSpeech);
        }

        private void ProcessSpeech(SFSpeechRecognitionResult result, NSError err)
        {
            if (err == null)
            {
                if (result.Transcriptions.Length > 0)
                {
                    Debug.WriteLine(result.Transcriptions[0].FormattedString);
                }
            }
        }

        public void SetLanguage(string language)
        {
            this.language= Langages.GetLanguage(language);
        }

      
    }

}
