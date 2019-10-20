namespace FancyTodoList
{
    public static class Constants
    {
        public static readonly string AuthenticationTokenEndpoint = "https://fancytodolisttraslator.cognitiveservices.azure.com/sts/v1.0";
        public static readonly string AuthenticationVozTokenEndpoint = "https://eastus.api.cognitive.microsoft.com/sts/v1.0";

        public static readonly string BingSpeechApiKey = "6cb5709405824b7ab1241244616c85e3";
        public static readonly string SpeechRecognitionEndpoint = "https://speech.platform.bing.com/speech/recognition/";
        public static readonly string AudioContentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

        public static readonly string BingSpellCheckApiKey = "84ce04e38dfd45d2a2a2a08dd9664b03";
        public static readonly string BingSpellCheckEndpoint = "https://api.cognitive.microsoft.com/bing/v7.0/SpellCheck";

        public static readonly string TextTranslatorApiKey = "19a871084a52457c8bfb5063df876085";
        public static readonly string TextTranslatorEndpoint = "https://api.microsofttranslator.com/v2/http.svc/translate";

		public static readonly string FaceApiKey = "02890de84ab441bab7493595258a2663";
        public static readonly string FaceEndpoint = "https://eastus.api.cognitive.microsoft.com/face/v1.0";

        public static readonly string AudioFilename = "Todo.wav";
    }
}
