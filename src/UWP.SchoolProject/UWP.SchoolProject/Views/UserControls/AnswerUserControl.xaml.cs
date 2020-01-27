using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWP.SchoolProject.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace UWP.SchoolProject.Views.UserControls
{
    public sealed partial class AnswerUserControl : UserControl
    {


        public AiAnswer Answer
        {
            get { return (AiAnswer)GetValue(AnswerProperty); }
            set { SetValue(AnswerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Answer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnswerProperty =
            DependencyProperty.Register("Answer", typeof(AiAnswer), typeof(AnswerUserControl), null);









        public AnswerUserControl()
        {
            this.InitializeComponent();
        }
    }
}
