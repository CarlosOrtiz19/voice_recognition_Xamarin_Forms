using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using System;
using System.Collections.Generic;
using TP2V3;
using System.Diagnostics;
using Debug = System.Diagnostics.Debug;

[assembly: Xamarin.Forms.Dependency(typeof(SpeechToTextAndroid))]
namespace TP2V3
{
    public class SpeechToTextAndroid : Java.Lang.Object, IRecognitionListener, ISpeechToText
    {
        // Speech
        public event EventHandler<string> OnSpeechResult;
        private SpeechRecognizer m_recognizer;
        private Intent m_voiceIntent;
        private string language;

        public SpeechToTextAndroid()
        {
            m_voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraPartialResults, true);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 0);
            m_voiceIntent.PutExtra(RecognizerIntent.ExtraCallingPackage, "ca.qc.claurendeau.speechy");
        }

        public void StartASR()
        {
            if (m_recognizer == null)
            {
                m_recognizer = SpeechRecognizer.CreateSpeechRecognizer(Android.App.Application.Context);
                m_recognizer.SetRecognitionListener(this);
                m_voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, language);
                Debug.WriteLine(language + " langage desde android");
                m_recognizer.StartListening(m_voiceIntent);
            }
        }

        public void StopASR()
        {
            if (m_recognizer != null)
            {
                m_recognizer.StopListening();
            }
        }

        public void SetLanguage(string language)
        {
            this.language= Langages.GetLanguage(language);
        }

        // ------------------------------------- HANDLERS ------------------------------------- //


        public void OnBeginningOfSpeech()
        {
            // Up to you
        }

        public void OnBufferReceived(byte[] buffer)
        {
            // Up to you
        }

        public void OnEndOfSpeech()
        {
            // Up to you
        }

        public void OnError([GeneratedEnum] SpeechRecognizerError error)
        {
            m_recognizer.StopListening();
            m_recognizer.Destroy();
            m_recognizer = null;
            switch (error)
            {
                case SpeechRecognizerError.Network:
                case SpeechRecognizerError.NetworkTimeout:
                case SpeechRecognizerError.Server:
                    // Up to you
                    break;
            }
        }

        public void OnEvent(int eventType, Bundle @params)
        {
            // Up to you
        }

        public void OnPartialResults(Bundle partialResults)
        {
            // Up to you
        }

        public void OnReadyForSpeech(Bundle @params)
        {
            // Up to you
        }

        public void OnResults(Bundle results)
        {
            IList<string> matches = results.GetStringArrayList(SpeechRecognizer.ResultsRecognition);
            if (matches.Count > 0)
            {
                OnSpeechResult(this, matches[0]);
                
            }
            m_recognizer.Destroy();
            m_recognizer = null;
        }

        public void OnRmsChanged(float rmsdB)
        {
            // Nothing for now
        }

    }

}
