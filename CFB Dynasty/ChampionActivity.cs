using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFB_Dynasty
{
    [Activity(Label = "ChampionActivity")]
    public class ChampionActivity : Activity
    {
        public static string CHAMPIONSHIPS = "championships";//Tag for saving total championships

        Button continueButton;

        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.ChampionLayout);

            continueButton = FindViewById<Button>(Resource.Id.championcontinue_button);

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            ISharedPreferencesEditor editor = prefs.Edit();

            int championships = prefs.GetInt(CHAMPIONSHIPS, 0);
            championships++;
            editor.PutInt(CHAMPIONSHIPS, championships);
            editor.Apply();

            continueButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(FinalStatsActivity));
                StartActivity(intent);
            };
        }
    }
}