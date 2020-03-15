using System;
using System.ComponentModel;

namespace TP2V3
{
    public class Langages {

        private  static string GetFrancais { get { return "fr-CA"; } }
        private  static string GetAnglais { get { return "en-US"; } }
        private  static string GetEspagnol { get { return "es-CO"; } }

        public static string GetLanguage(string language) {

            switch (language)
            {
                case "EN":
                    return "en-US";
                case "FR":
                    return "fr-CA";
                case "ES":
                    return "es-CO";
                default:
                    return "pas reconue";
            }
        }
    }
    public interface ISpeechToText
    {
        
        void StartASR();
        void StopASR();

        event EventHandler<string> OnSpeechResult;

        void SetLanguage(string language);

    }
}