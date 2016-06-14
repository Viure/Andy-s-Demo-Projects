using Xamarin.Forms;

namespace CortanaDemo
{
    public partial class EditorPage : ContentPage, IEditor
    {
       

        public EditorPage(string InitialText)
        {
            InitializeComponent();
           
            editor.Text = InitialText;
        }

        public static EditorPage GetEditorPage(string InitialText)
        {
            return new EditorPage(InitialText);
            
        }

        protected Label Instructions
        {
            get { return instructions; }
        }

        protected Editor EditControl
        {
            get
            {
                return editor;
            }
        }
        protected Button StartButton
        {
            get
               { return startButton; }
        }

        protected Button StopButton
        {
            get { return stopButton; }
        }

        protected Button CancelButton
        {
            get { return cancelButton; }
        }

    }
}
