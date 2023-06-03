using Android.App;
using Android.Content;
using Android.Nfc;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CFB_Dynasty
{
    [Activity(Label = "FinalsActivity")]
    public class FinalsActivity : Activity
    {
        public static string TAG = "FinalsActivity";//Tag for logging

        ImageView userImg;
        ImageView oppImg;
        TextView userName;
        TextView oppName;
        TextView userScore;
        TextView oppScore;
        Button simulateButton;
        Button continueButton;

        int userscore = 0;
        int oppscore = 0;

        bool played = false;
        bool champion = false;

        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.FinalsLayout);

            userImg = FindViewById<ImageView>(Resource.Id.finalsuser_image);
            oppImg = FindViewById<ImageView>(Resource.Id.finalsopp_image);
            userName = FindViewById<TextView>(Resource.Id.finalsuser_name);
            oppName = FindViewById<TextView>(Resource.Id.finalsopp_name);
            userScore = FindViewById<TextView>(Resource.Id.finalsuser_score);
            oppScore = FindViewById<TextView>(Resource.Id.finalsopp_score);
            simulateButton = FindViewById<Button>(Resource.Id.finalsimulate_button);
            continueButton = FindViewById<Button>(Resource.Id.finalscontinue_button);

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            userName.Text = prefs.GetString(TeamActivity.TEAM, null);
            userImg.SetImageResource(prefs.GetInt(TeamActivity.IMG_NUM, 0));

            Intent intent = Intent;
            oppName.Text = intent.GetStringExtra(SemifinalActivity.FINALSOPP);
            oppImg.SetImageResource(intent.GetIntExtra(SemifinalActivity.FINALSOPPIMG, 0));

            simulateButton.Click += delegate
            {
                if (!played)
                {
                    SimulateButton_Click();
                }
                else
                {
                    Toast.MakeText(this, "This game has been played!", ToastLength.Short).Show();
                }
            };

            continueButton.Click += delegate
            {
                Intent intent2;
                if (champion)
                {
                    intent2 = new Intent(this, typeof(ChampionActivity));
                    StartActivity(intent2);
                }
                else if (!played)
                {
                    Toast.MakeText(this, "This game has not been played!", ToastLength.Short).Show();
                }
                else 
                { 
                    intent2 = new Intent(this, typeof(FinalStatsActivity));
                    StartActivity(intent2);
                }
            };
        }

        private void SimulateButton_Click()
        {
            played = true;

            var lowerBound = 0;
            var upperBound = 8;
            while (userscore == oppscore)
            {
                userscore = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
                oppscore = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
            }

            userScore.Text = userscore.ToString();
            oppScore.Text = oppscore.ToString();

            ISharedPreferencesEditor editor = prefs.Edit();

            int savecuruserscore = prefs.GetInt(MatchupActivity.CURUSERSCORE, 0);
            int savecuroppscore = prefs.GetInt(MatchupActivity.CUROPPSCORE, 0);
            int savealluserscore = prefs.GetInt(MatchupActivity.ALLUSERSCORE, 0);
            int savealloppscore = prefs.GetInt(MatchupActivity.ALLOPPSCORE, 0);
            savecuruserscore += userscore;
            savecuroppscore += oppscore;
            savealluserscore += userscore;
            savealloppscore += oppscore;
            editor.PutInt(MatchupActivity.CURUSERSCORE, savecuruserscore);
            editor.PutInt(MatchupActivity.CUROPPSCORE, savecuroppscore);
            editor.PutInt(MatchupActivity.ALLUSERSCORE, savealluserscore);
            editor.PutInt(MatchupActivity.ALLOPPSCORE, savealloppscore);

            if (userscore > oppscore)
            {
                int curwins = prefs.GetInt(MatchupActivity.CURWINS, 0);
                int allwins = prefs.GetInt(MatchupActivity.ALLWINS, 0);
                curwins++;
                allwins++;
                editor.PutInt(MatchupActivity.CURWINS, curwins);
                editor.PutInt(MatchupActivity.ALLWINS, allwins);
                champion = true;
            }
            else if (userscore < oppscore)
            {
                int curloss = prefs.GetInt(MatchupActivity.CURLOSS, 0);
                int allloss = prefs.GetInt(MatchupActivity.ALLLOSS, 0);
                curloss++;
                allloss++;
                editor.PutInt(MatchupActivity.CURLOSS, curloss);
                editor.PutInt(MatchupActivity.ALLLOSS, allloss);
            }
            editor.Apply();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            Log.Debug(TAG, "OnSavedInstance Called");
        }
    }
}