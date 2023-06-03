using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.ComponentModel;
using Newtonsoft.Json;
using AndroidX.Preference;
using System.IO;

namespace CFB_Dynasty
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.AppCompat.Light", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        static string TAG = "MainActivity"; //Tag for logging MainActivity
        public static string SAVE = "save";//Tag for save flag

        Button playButton;
        Button resetButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            Log.Info(TAG, "OnCreate(Bundle) called");

            //Set Buttons
            playButton = FindViewById<Button>(Resource.Id.play_button);
            resetButton = FindViewById<Button>(Resource.Id.reset_button);

            playButton.Click += delegate
            {
                //Check if save exists
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                bool hasSave = prefs.GetBoolean("save", false);
                int totalgames = prefs.GetInt(MatchupActivity.TOTALGAMES, 0);
                if (!hasSave)
                {
                    var intent = new Intent(this, typeof(SelectTeamActivity));
                    StartActivity(intent);
                }
                else if (totalgames < 12)
                {
                    var intent = new Intent(this, typeof(MatchupActivity));
                    StartActivity(intent);
                }
                else
                {
                    var intent = new Intent(this, typeof(SemifinalActivity));
                }
            };

            resetButton.Click += delegate
            {
                //Check if save exists
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                bool hasSave = prefs.GetBoolean("save", false);
                if (!hasSave)
                {
                    Toast.MakeText(this, Resource.String.no_save_toast, ToastLength.Short).Show();
                }
                else
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.Clear();
                    editor.Apply();
                    editor.PutBoolean(SAVE, false);
                    editor.Apply();
                    Toast.MakeText(this, Resource.String.reset_toast, ToastLength.Short).Show();
                }
            };
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}