using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Preference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFB_Dynasty
{
    [Activity(Label = "FinalStatsActivity")]
    public class FinalStatsActivity : Activity
    {
        ISharedPreferences prefs;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.FinalStatsLayout);

            Button continueButton = FindViewById<Button>(Resource.Id.finalstats_continue);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            int curwins = prefs.GetInt(MatchupActivity.CURWINS, 0);
            int curloss = prefs.GetInt(MatchupActivity.CURLOSS, 0);
            int curtie = prefs.GetInt(MatchupActivity.CURTIE, 0);
            int curuserscore = prefs.GetInt(MatchupActivity.CURUSERSCORE, 0);
            int curoppscore = prefs.GetInt(MatchupActivity.CUROPPSCORE, 0);

            TextView curWins = FindViewById<TextView>(Resource.Id.curWin);
            TextView curLoss = FindViewById<TextView>(Resource.Id.curLoss);
            TextView curTie = FindViewById<TextView>(Resource.Id.curTie);
            TextView curUserScore = FindViewById<TextView>(Resource.Id.curUserScore);
            TextView curOppScore = FindViewById<TextView>(Resource.Id.curOppScore);

            curWins.Text = curwins.ToString();
            curLoss.Text = curloss.ToString();
            curTie.Text = curtie.ToString();
            curUserScore.Text = curuserscore.ToString();
            curOppScore.Text = curoppscore.ToString();

            continueButton.Click += delegate
            {
                ClearSeasonStats();
                Intent intent = new Intent(this, typeof(MatchupActivity));
                StartActivity(intent);
            };
        }

        private void ClearSeasonStats()
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutInt(MatchupActivity.CURWINS, 0);
            editor.PutInt(MatchupActivity.CURLOSS, 0);
            editor.PutInt(MatchupActivity.CURTIE, 0);
            editor.PutInt(MatchupActivity.CURUSERSCORE, 0);
            editor.PutInt(MatchupActivity.CUROPPSCORE, 0);
            editor.PutInt(MatchupActivity.TOTALGAMES, 0);
            editor.PutBoolean(SemifinalActivity.INFINALS, false);
            editor.Apply();
        }
    }
}