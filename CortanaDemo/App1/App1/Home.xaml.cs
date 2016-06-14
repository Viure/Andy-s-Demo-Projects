using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CortanaDemo
{
    public partial class Home : ContentPage
    {

        public delegate ContentPage GetEditorInstance(string InitialEditorText);
        static public GetEditorInstance EditorFactory;
       
        public Home()
        {
            InitializeComponent();

            if (EditorFactory == null)
            {
                EditorFactory = EditorPage.GetEditorPage;
            }
        }

        public void OnEditorStart (Object sender, EventArgs args)
        {
            Navigation.PushAsync(EditorFactory ("Some existing text\n\n"));
        }
    }
}
