using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CalorieCounter
{
    /// <summary>
    /// A custom Stepper for increasing/decreasing item count on AddItemsPopUp page
    /// </summary>
    public class CustomStepper : StackLayout
    {
        Button PlusBtn;
        Button MinusBtn;

        //https://forums.xamarin.com/discussion/140026/stepper-in-xamarin-forms
        public static readonly BindableProperty TextProperty =
          BindableProperty.Create(
             propertyName: "Text",
              returnType: typeof(int),
              declaringType: typeof(CustomStepper),
              defaultValue: 1,
              defaultBindingMode: BindingMode.TwoWay);

        public int Text
        {
            get { return (int)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public CustomStepper()
        {
            PlusBtn = new Button {
                Text = "+",
                WidthRequest = 30,
                Margin = 0,
                CornerRadius = 5,
                BackgroundColor = Color.FromHex("DDA448"),
                TextColor = Color.Black,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold

            };
            MinusBtn = new Button { 
                Text = "-", 
                WidthRequest = 30, 
                Margin = 0, 
                CornerRadius = 5,
                BackgroundColor = Color.FromHex("DDA448"),
                TextColor = Color.Black,
                FontSize = 13,
                FontAttributes = FontAttributes.Bold
            };

            Orientation = StackOrientation.Horizontal;
            Spacing = 1;
            Margin = new Thickness (0,3,0,3);
            HorizontalOptions = LayoutOptions.Center;
            VerticalOptions = LayoutOptions.Center;
            PlusBtn.Clicked += PlusBtn_Clicked;
            MinusBtn.Clicked += MinusBtn_Clicked;
          
            Children.Add(MinusBtn);
            Children.Add(PlusBtn);
        }

        private void MinusBtn_Clicked(object sender, EventArgs e)
        {
            if (Text > 0)
                Text--;
        }

        private void PlusBtn_Clicked(object sender, EventArgs e)
        {
            Text++;
        }
    }
}
