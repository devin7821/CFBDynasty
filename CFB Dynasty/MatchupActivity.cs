using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.Database;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using AndroidX.AppCompat.App;
using Android.Nfc;
using Android.Util;

namespace CFB_Dynasty
{
    [Activity(Label = "MatchupActivity")]
    public class MatchupActivity : AppCompatActivity
    {
        static string TAG = "MatchupActivity";//Tag for logging
        static string SAVEOPP = "opp";//Tag for saving opponent in savedinstance
        static string SAVEOPPIMG = "oppimg";//Tag for saving opponent img in savedinstance
        static string SAVEOPPSCORE = "oppscore";//Tag for saving opponent score in savedinstance
        static string SAVEUSERSCORE = "userscore";//Tag for saving user score in savedinstance

        public static string CURWINS = "curwins";//Tag for saving current wins
        public static string CURLOSS = "curloss";//Tag for saving current losses
        public static string CURTIE = "curtie";//TAG for saving current ties
        public static string CURUSERSCORE = "curuserscore";//Tag for saving current user score
        public static string CUROPPSCORE = "curoppscore";//Tag for saving opponent score
        public static string ALLWINS = "allwins";//Tag for saving all wins
        public static string ALLLOSS = "allloss";//Tag for saving all losses
        public static string ALLTIE = "alltie";//Tag for saving all ties
        public static string ALLUSERSCORE = "alluserscore";//Tag for saving all user score
        public static string ALLOPPSCORE = "alloppscore";//Tag for saving all opponent score
        public static string TOTALGAMES = "totalgames";//Tag for saving game count

        bool played = false;

        ImageView userImage;
        ImageView oppImage;
        TextView userName;
        TextView oppName;
        TextView userScore;
        TextView oppScore;

        Button simulateButton;
        Button statsButton;
        Button continueButton;

        string opp;
        int oppimg;
        int oppscore;
        int userscore;
        int totalgames;

        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here             
            SetContentView(Resource.Layout.MatchupLayout);
            View fragmentContainer = FindViewById(Resource.Id.statsfragcontainer);
            if (fragmentContainer != null)
            {
                LoadStatsFragment();
            }

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            string teamName = prefs.GetString(TeamActivity.TEAM, null);
            int image = prefs.GetInt(TeamActivity.IMG_NUM, 0);
            totalgames = prefs.GetInt(TOTALGAMES, 0);

            userImage = FindViewById<ImageView>(Resource.Id.user_image);
            oppImage = FindViewById<ImageView>(Resource.Id.opp_image);
            userName = FindViewById<TextView>(Resource.Id.user_name);
            oppName = FindViewById<TextView>(Resource.Id.opp_name);
            userScore = FindViewById<TextView>(Resource.Id.user_score);
            oppScore = FindViewById<TextView>(Resource.Id.opp_score);

            simulateButton = FindViewById<Button>(Resource.Id.simulate_button);
            statsButton = FindViewById<Button>(Resource.Id.stats_button);
            continueButton = FindViewById<Button>(Resource.Id.continue_button);

            userImage.SetImageResource(image); //column 2
            userName.Text = teamName;

            if (savedInstanceState == null)
            {
                NewOpponent();
            }
            else
            {
                oppName.Text = savedInstanceState.GetString(SAVEOPP, String.Empty);
                oppImage.SetImageResource(savedInstanceState.GetInt(SAVEOPPIMG, 0));
                oppScore.Text = savedInstanceState.GetInt(SAVEOPPSCORE, 0).ToString();
                userScore.Text = savedInstanceState.GetInt(SAVEUSERSCORE, 0).ToString();
            }

            simulateButton.Click += delegate
            {
                if (!played)
                {
                    SimulateButton_Click();
                    played = true;
                    if (fragmentContainer != null)
                    {
                        LoadStatsFragment();
                    }
                }
                else 
                {
                    Toast.MakeText(this, "This game has been played!", ToastLength.Short).Show();
                }
            };

            statsButton.Click += delegate
            {
                Intent intent = new Intent(this, typeof(StatsActivity));
                StartActivity(intent);
            };

            continueButton.Click += delegate
            {
                if (!played)
                {
                    Toast.MakeText(this, "This game has not been played!", ToastLength.Short).Show();
                }
                else if (totalgames < 12)
                {
                    NewOpponent();
                    played = false;
                }
                else
                {
                    Intent intent = new Intent(this, typeof(SemifinalActivity));
                    StartActivity(intent);
                }
            };
        }

        private void NewOpponent()
        {
            try
            {
                var lowerBound = 1;
                var upperBound = 17;
                int rnum = 0;
                while (rnum == 0 || rnum == prefs.GetInt(TeamActivity.DB_NUM, 0))
                {
                    rnum = RandomNumberGenerator.GetInt32(lowerBound, upperBound);
                }

                SQLiteOpenHelper teamHelper = new TeamDatabaseHelper(this);
                SQLiteDatabase db = teamHelper.ReadableDatabase;

                ICursor cursor = db.Query("TEAM",
                    new string[] { "NAME", "IMAGE_RESOURCE_ID" },
                    "_id = ?", new string[] { rnum.ToString() }, null, null, null); //"_id = ?" where the contents equal mealNum

                if (cursor.MoveToFirst()) //must do
                {
                    opp = cursor.GetString(0);
                    oppimg = cursor.GetInt(1);
                    oppImage.SetImageResource(oppimg); //column 2
                    oppName.Text = opp;
                }
            }
            catch (SQLException e)
            {
                Toast.MakeText(this, "TeamActivityDbase unavailable", ToastLength.Short).Show();
            }

            userScore.Text = "0";
            oppScore.Text = "0";
        }

        private void LoadStatsFragment()
        {
            StatsFragClass statsdetailsfrag = new StatsFragClass();
            AndroidX.Fragment.App.FragmentTransaction ft = SupportFragmentManager.BeginTransaction();
            ft.Replace(Resource.Id.statsfragcontainer, statsdetailsfrag);
            ft.AddToBackStack(null);
            ft.SetTransition((int)FragmentTransit.FragmentClose);
            ft.Commit();
        }

        private void SimulateButton_Click()
        {
            var lowerBound = 0;
            var upperBound = 8;
            userscore = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
            oppscore = RandomNumberGenerator.GetInt32(lowerBound,upperBound) * 7;

            userScore.Text = userscore.ToString();
            oppScore.Text = oppscore.ToString();

            ISharedPreferencesEditor editor = prefs.Edit();

            int savecuruserscore = prefs.GetInt(CURUSERSCORE, 0);
            int savecuroppscore = prefs.GetInt(CUROPPSCORE, 0);
            int savealluserscore = prefs.GetInt(ALLUSERSCORE, 0);
            int savealloppscore = prefs.GetInt(ALLOPPSCORE, 0);
            savecuruserscore += userscore;
            savecuroppscore += oppscore;
            savealluserscore += userscore;
            savealloppscore += oppscore;
            totalgames++;
            editor.PutInt(CURUSERSCORE, savecuruserscore);
            editor.PutInt(CUROPPSCORE, savecuroppscore);
            editor.PutInt(ALLUSERSCORE, savealluserscore);
            editor.PutInt(ALLOPPSCORE, savealloppscore);
            editor.PutInt(TOTALGAMES, totalgames);

            if (userscore > oppscore)
            {
                int curwins = prefs.GetInt(CURWINS, 0);
                int allwins = prefs.GetInt(ALLWINS, 0);
                curwins++;
                allwins++;
                editor.PutInt(CURWINS, curwins);
                editor.PutInt(ALLWINS, allwins);
            }
            else if (userscore < oppscore)
            {
                int curloss = prefs.GetInt(CURLOSS, 0);
                int allloss = prefs.GetInt(ALLLOSS, 0);
                curloss++;
                allloss++;
                editor.PutInt(CURLOSS, curloss);
                editor.PutInt(ALLLOSS, allloss);
            }
            else
            {
                int curties = prefs.GetInt(CURTIE, 0);
                int allties = prefs.GetInt(ALLTIE, 0);
                curties++;
                allties++;
                editor.PutInt(CURTIE, curties);
                editor.PutInt(ALLTIE, allties);
            }
            editor.Apply();
        }

        protected override void OnStart()
        {
            base.OnStart();
            Log.Debug(TAG, "OnStart Called");
        }
        protected override void OnResume()
        {
            base.OnResume();
            Log.Debug(TAG, "OnResume Called");
        }
        protected override void OnPause()
        {
            base.OnPause();
            Log.Debug(TAG, "OnPause Called");
        }
        protected override void OnStop()
        {
            base.OnStop();
            Log.Debug(TAG, "OnStop Called");
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            Log.Debug(TAG, "OnDestroy Called");
        }
        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            Log.Debug(TAG, "OnSavedInstance Called");
            outState.PutString(SAVEOPP, opp);
            outState.PutInt(SAVEOPPIMG, oppimg);
            outState.PutInt(SAVEOPPSCORE, oppscore);
            outState.PutInt(SAVEUSERSCORE, userscore);
        }
    }
}