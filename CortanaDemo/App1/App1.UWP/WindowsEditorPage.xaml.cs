using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using Windows.Media.SpeechRecognition;
using Windows.UI.Core;
using System.Diagnostics;
using System.Threading;

namespace CortanaDemo.UWP
{
    public class WindowsEditorPage1 : EditorPage, IEditor
    {

        SpeechRecognizer rec;
        CoreDispatcher dispatcher;
        SpeechRecognitionCompilationResult grammar = null;
        string originalEditorText;
        StringBuilder dictatedText = null;

        public WindowsEditorPage1(string InitialText) : base(InitialText)
        {

            rec = new SpeechRecognizer();
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            StartButton.IsVisible = true;
            StartButton.Clicked += OnStartClicked;
            StopButton.IsVisible = true;
            StopButton.IsEnabled = false;
            StopButton.Clicked += OnStopClicked;
            CancelButton.IsVisible = true;
            CancelButton.IsEnabled = false;
            CancelButton.Clicked += OnCancelClicked;
            Instructions.Text = "You can type or press start to dictate text.";

            rec.ContinuousRecognitionSession.Completed += OnRecognitionSessionCompleted;
            rec.ContinuousRecognitionSession.ResultGenerated += OnRecognitionSessionResultGenerated;
            rec.HypothesisGenerated += OnRecognationHypothesisGenerated;

        }

        public static WindowsEditorPage1 GetWindowsEditorPage(string InitialText)
        {
            return new WindowsEditorPage1(InitialText);
        }

        public async void OnStartClicked(Object sender, EventArgs args)
        {

            if (grammar == null)
                grammar = await rec.CompileConstraintsAsync();

            if (dictatedText == null)
                dictatedText = new StringBuilder();
            else
                dictatedText.Clear();

            originalEditorText = EditControl.Text;

            if (rec.State == SpeechRecognizerState.Idle)
            {
                await rec.ContinuousRecognitionSession.StartAsync();
                StartButton.IsEnabled = false;
                StopButton.IsEnabled = true;
                CancelButton.IsEnabled = true;
            }

        }


        public async void OnStopClicked(Object sender, EventArgs args)
        {

            if (rec.State != SpeechRecognizerState.Idle)
            {
                await rec.ContinuousRecognitionSession.StopAsync();
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                CancelButton.IsEnabled = false;
                Debug.WriteLine("stoped");
            }
            else
                Debug.WriteLine("already idle");

        }

        public async void OnCancelClicked(Object sender, EventArgs args)
        {

            if (rec.State != SpeechRecognizerState.Idle)
            {
                await rec.ContinuousRecognitionSession.CancelAsync();
                StartButton.IsEnabled = true;
                StopButton.IsEnabled = false;
                CancelButton.IsEnabled = false;
                Debug.WriteLine("cancelled");
            }
            else
                Debug.WriteLine("already idle");

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                EditControl.Text = originalEditorText;
            });

        }

        private async void OnRecognitionSessionCompleted(
            SpeechContinuousRecognitionSession sender,
            SpeechContinuousRecognitionCompletedEventArgs args)
        {
            Debug.WriteLine("OnRecognitionSessionCompleted status = " + args.Status.ToString());

            if (args.Status == SpeechRecognitionResultStatus.UserCanceled)
            {
                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    EditControl.Text = originalEditorText + dictatedText.ToString();
                });
            }

            
        }

        private async void OnRecognitionSessionResultGenerated(
            SpeechContinuousRecognitionSession sender,
            SpeechContinuousRecognitionResultGeneratedEventArgs args)
        {
            Debug.WriteLine("OnRecognitionSessionResultGenerated condifence = {0} Text = {1}",
                args.Result.Confidence.ToString(), args.Result.Text);

            dictatedText.Append(args.Result.Text);

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                EditControl.Text = originalEditorText + dictatedText.ToString();
            });
        }
    
        private async void OnRecognationHypothesisGenerated(
            SpeechRecognizer sender,
            SpeechRecognitionHypothesisGeneratedEventArgs args)
        {

            Debug.WriteLine("Hypotheses generated = " + args.Hypothesis.Text);

            

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                EditControl.Text = originalEditorText + args.Hypothesis.Text + "...";
            });
        }
    }
}
