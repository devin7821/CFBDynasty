using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.Database;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
using Android.Nfc;
using Android.Util;

namespace CFB_Dynasty
{
    [Activity(Label = "SemifinalActivity")]
    public class SemifinalActivity : Activity
    {
        public static string TAG = "SemifinalActivity";//Tag for logging
        public static string INFINALS = "infinals";//Tag for identifying the user as in the finals
        public static string FINALSOPP = "finalsopp";//Tag for transferring finals opponent
        public static string FINALSOPPIMG = "finalsoppimg";//Tag for transferring finals opponent image

        ImageView userImg;
        ImageView oppImg1;
        ImageView oppImg2;
        ImageView oppImg3;
        TextView userName;
        TextView oppName1;
        TextView oppName2;
        TextView oppName3;
        TextView userScore;
        TextView oppScore1;
        TextView oppScore2;
        TextView oppScore3;
        Button simulateButton;
        Button continueButton;       

        string opp1;
        string opp2;
        string opp3;
        string oppwinner;
        int oppimg1;
        int oppimg2;
        int oppimg3;
        int oppwinnerimg;
        int userscore = 0;
        int oppscore1 = 0;
        int oppscore2 = 0;
        int oppscore3 = 0;

        bool infinals = false;
        bool played = false;

        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.SemifinalLayout);

            userImg = FindViewById<ImageView>(Resource.Id.playoffuser_image);
            oppImg1 = FindViewById<ImageView>(Resource.Id.playoffopp_img1);
            oppImg2 = FindViewById<ImageView>(Resource.Id.playoffopp_img2);
            oppImg3 = FindViewById<ImageView>(Resource.Id.playoffopp_img3);
            userName = FindViewById<TextView>(Resource.Id.playoffuser_name);
            oppName1 = FindViewById<TextView>(Resource.Id.playoffopp_name1);
            oppName2 = FindViewById<TextView>(Resource.Id.playoffopp_name2);
            oppName3 = FindViewById<TextView>(Resource.Id.playoffopp_name3);
            userScore = FindViewById<TextView>(Resource.Id.playoffuser_score);
            oppScore1 = FindViewById<TextView>(Resource.Id.playoffopp_score1);
            oppScore2 = FindViewById<TextView>(Resource.Id.playoffopp_score2);
            oppScore3 = FindViewById<TextView>(Resource.Id.playoffopp_score3);
            simulateButton = FindViewById<Button>(Resource.Id.playoff_simulate);
            continueButton = FindViewById<Button>(Resource.Id.playoff_continue);


            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            userName.Text = prefs.GetString(TeamActivity.TEAM, null);
            userImg.SetImageResource(prefs.GetInt(TeamActivity.IMG_NUM, 0));

            GetOpponents();

            simulateButton.Click += delegate
            {
                if (!played)
                {
                    SimulateGames();
                }
                else
                {
                    Toast.MakeText(this, "This game has been played!", ToastLength.Short).Show();
                }
            };

            continueButton.Click += delegate
            {
                if (infinals) 
                {
                    Intent intent = new Intent(this, typeof(FinalsActivity));
                    intent.PutExtra(FINALSOPP, oppwinner);
                    intent.PutExtra(FINALSOPPIMG, oppwinnerimg);
                    StartActivity(intent);
                }
                else if (!played)
                {
                    Toast.MakeText(this, "This game has not been played!", ToastLength.Short).Show();
                }
                else
                {
                    Intent intent = new Intent(this, typeof(FinalStatsActivity));
                    StartActivity(intent);
                }
            };
        }

        private void SimulateGames()
        {
            played = true;

            var lowerBound = 0;
            var upperBound = 8;
            while (userscore == oppscore2 || oppscore1 == oppscore3)
            {
                userscore = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
                oppscore1 = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
                oppscore2 = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
                oppscore3 = RandomNumberGenerator.GetInt32(lowerBound, upperBound) * 7;
            }

            userScore.Text = userscore.ToString();
            oppScore1.Text = oppscore1.ToString();
            oppScore2.Text = oppscore2.ToString();
            oppScore3.Text = oppscore3.ToString();

            ISharedPreferencesEditor editor = prefs.Edit();

            int savecuruserscore = prefs.GetInt(MatchupActivity.CURUSERSCORE, 0);
            int savecuroppscore = prefs.GetInt(MatchupActivity.CUROPPSCORE, 0);
            int savealluserscore = prefs.GetInt(MatchupActivity.ALLUSERSCORE, 0);
            int savealloppscore = prefs.GetInt(MatchupActivity.ALLOPPSCORE, 0);
            savecuruserscore += userscore;
            savecuroppscore += oppscore2;
            savealluserscore += userscore;
            savealloppscore += oppscore2;
            editor.PutInt(MatchupActivity.CURUSERSCORE, savecuruserscore);
            editor.PutInt(MatchupActivity.CUROPPSCORE, savecuroppscore);
            editor.PutInt(MatchupActivity.ALLUSERSCORE, savealluserscore);
            editor.PutInt(MatchupActivity.ALLOPPSCORE, savealloppscore);

            if (userscore > oppscore2)
            {
                int curwins = prefs.GetInt(MatchupActivity.CURWINS, 0);
                int allwins = prefs.GetInt(MatchupActivity.ALLWINS, 0);
                curwins++;
                allwins++;
                editor.PutInt(MatchupActivity.CURWINS, curwins);
                editor.PutInt(MatchupActivity.ALLWINS, allwins);
                editor.PutBoolean(INFINALS, true);
                infinals = true;
            }
            else if (userscore < oppscore2)
            {
                int curloss = prefs.GetInt(MatchupActivity.CURLOSS, 0);
                int allloss = prefs.GetInt(MatchupActivity.ALLLOSS, 0);
                curloss++;
                allloss++;
                editor.PutInt(MatchupActivity.CURLOSS, curloss);
                editor.PutInt(MatchupActivity.ALLLOSS, allloss);
            }
            
            if (oppscore1 > oppscore3)
            {
                oppwinner = opp1;
                oppwinnerimg = oppimg1;
            }
            else if (oppscore1 < oppscore3)
            {
                oppwinner = opp3;
                oppwinnerimg = oppimg3;
            }
            editor.Apply();
        }

        private void GetOpponents()
        {
            int found1 = 0;
            try
            {
                var lowerBound = 1;
                var upperBound = 17;
                while (found1 == 0 || found1 == prefs.GetInt(TeamActivity.DB_NUM, 0))
                {
                    found1 = RandomNumberGenerator.GetInt32(lowerBound, upperBound);
                }

                SQLiteOpenHelper teamHelper = new TeamDatabaseHelper(this);
                SQLiteDatabase db = teamHelper.ReadableDatabase;

                ICursor cursor = db.Query("TEAM",
                    new string[] { "NAME", "IMAGE_RESOURCE_ID" },
                    "_id = ?", new string[] { found1.ToString() }, null, null, null); //"_id = ?" where the contents equal mealNum

                if (cursor.MoveToFirst()) //must do
                {
                    opp1 = cursor.GetString(0);
                    oppimg1 = cursor.GetInt(1);
                    oppImg1.SetImageResource(oppimg1); //column 2
                    oppName1.Text = opp1;
                }
            }
            catch (SQLException e)
            {
                Toast.MakeText(this, "TeamActivityDbase unavailable", ToastLength.Short).Show();
            }

            int found2 = 0;
            try
            {
                var lowerBound = 1;
                var upperBound = 5;
                while (found2 == 0 || found2 == prefs.GetInt(TeamActivity.DB_NUM, 0) || found2 == found1)
                {
                    found2 = RandomNumberGenerator.GetInt32(lowerBound, upperBound);
                }

                SQLiteOpenHelper teamHelper = new TeamDatabaseHelper(this);
                SQLiteDatabase db = teamHelper.ReadableDatabase;

                ICursor cursor = db.Query("TEAM",
                    new string[] { "NAME", "IMAGE_RESOURCE_ID" },
                    "_id = ?", new string[] { found2.ToString() }, null, null, null); //"_id = ?" where the contents equal mealNum

                if (cursor.MoveToFirst()) //must do
                {
                    opp2 = cursor.GetString(0);
                    oppimg2 = cursor.GetInt(1);
                    oppImg2.SetImageResource(oppimg2); //column 2
                    oppName2.Text = opp2;
                }
            }
            catch (SQLException e)
            {
                Toast.MakeText(this, "TeamActivityDbase unavailable", ToastLength.Short).Show();
            }

            int found3 = 0;
            try
            {
                var lowerBound = 1;
                var upperBound = 5;
                while (found3 == 0 || found3 == prefs.GetInt(TeamActivity.DB_NUM, 0) || found3 == found1 || found3 == found2)
                {
                    found3 = RandomNumberGenerator.GetInt32(lowerBound, upperBound);
                }

                SQLiteOpenHelper teamHelper = new TeamDatabaseHelper(this);
                SQLiteDatabase db = teamHelper.ReadableDatabase;

                ICursor cursor = db.Query("TEAM",
                    new string[] { "NAME", "IMAGE_RESOURCE_ID" },
                    "_id = ?", new string[] { found3.ToString() }, null, null, null); //"_id = ?" where the contents equal mealNum

                if (cursor.MoveToFirst()) //must do
                {
                    opp3 = cursor.GetString(0);
                    oppimg3 = cursor.GetInt(1);
                    oppImg3.SetImageResource(oppimg3); //column 2
                    oppName3.Text = opp3;
                }
            }
            catch (SQLException e)
            {
                Toast.MakeText(this, "TeamActivityDbase unavailable", ToastLength.Short).Show();
            }
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            Log.Debug(TAG, "OnSavedInstance Called");
        }
    }
}