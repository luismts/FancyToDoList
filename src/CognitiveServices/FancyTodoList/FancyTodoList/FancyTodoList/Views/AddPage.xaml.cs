using FancyTodoList.Interfaces;
using FancyTodoList.Models;
using FancyTodoList.Services;
using FancyTodoList.ViewModels;
using Plugin.AudioRecorder;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FancyTodoList.Views
{
    public partial class AddPage : ContentPage
    {
        IBingSpeechService bingSpeechService;
        IBingSpellCheckService bingSpellCheckService;
        ITextTranslationService textTranslationService;
        AudioRecorderService audioRecordingService;

        AddPageViewModel Context;

        public AddPage()
        {
            InitializeComponent();
            Context = this.BindingContext as AddPageViewModel;

            bingSpeechService = new BingSpeechService(new AuthenticationService(Constants.BingSpeechApiKey), Device.RuntimePlatform);
            bingSpellCheckService = new BingSpellCheckService();
            textTranslationService = new TextTranslationService(new AuthenticationService(Constants.TextTranslatorApiKey));

            audioRecordingService = new AudioRecorderService();
        }

        void OnRecognizeSpeechButtonClicked(object sender, EventArgs e)
        {
            var x = audioRecordingService;
            Device.BeginInvokeOnMainThread(async () => {
                
                try
                {
                    Task<string> audioRecordTask = null;
                    if (!audioRecordingService.IsRecording)
                    {
                        audioRecordTask = await audioRecordingService.StartRecording();

                        activityIndicator.IsVisible = true;

                        spellsheckButton.IsEnabled = false;
                        translationButton.IsEnabled = false;

                        ((Button)sender).Image = "record.png";
                    }
                    else
                    {
                        await audioRecordingService.StopRecording();

                        ((Button)sender).Image = "recording.png";
                    }

                    //isRecording = !isRecording;
                    if (!audioRecordingService.IsRecording)
                    {
                        var audioFile = await audioRecordTask;

                        if (audioFile == null)
                            return;

                        var speechResult = await bingSpeechService.RecognizeSpeechAsync(audioFile, Constants.TextTranslatorApiKey);
                        Debug.WriteLine("Name: " + speechResult.DisplayText);
                        Debug.WriteLine("Recognition Status: " + speechResult.RecognitionStatus);

                        if (!string.IsNullOrWhiteSpace(speechResult.DisplayText))
                        {
                            Context.Item.Description = char.ToUpper(speechResult.DisplayText[0]) + speechResult.DisplayText.Substring(1);
                            OnPropertyChanged("TodoItem");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    if (!audioRecordingService.IsRecording)
                    {
                        //((Button)sender).Image = "record.png";

                        activityIndicator.IsVisible = false;
                        spellsheckButton.IsEnabled = true;
                        translationButton.IsEnabled = true;
                    }
                }
            });

        }

        async void OnSpellCheckButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Context.Item.Description))
                {
                    activityIndicator.IsVisible = true;

                    var spellCheckResult = await bingSpellCheckService.SpellCheckTextAsync(Context.Item.Description);
                    foreach (var flaggedToken in spellCheckResult.FlaggedTokens)
                    {
                        Context.Item.Description = Context.Item.Description.Replace(flaggedToken.Token, flaggedToken.Suggestions.FirstOrDefault().Suggestion);
                    }
                    OnPropertyChanged("TodoItem");

                    activityIndicator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        async void OnTranslateButtonClicked(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(Context.Item.Description))
                {
                    activityIndicator.IsVisible = true;

                    Context.Item.Description = await textTranslationService.TranslateTextAsync(Context.Item.Description);
                    OnPropertyChanged("TodoItem");

                    activityIndicator.IsVisible = false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
