using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CalorieCounter.Droid
{

    [Activity(NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = "com.googleusercontent.apps.1041253101002-dhan7880g5t577r7d6lc8cfcsvqfqqhf",
        DataPath = "/oauth2redirect")]
    public class WebAuthenticationCallback : Xamarin.Essentials.WebAuthenticatorCallbackActivity
    {
    }
}