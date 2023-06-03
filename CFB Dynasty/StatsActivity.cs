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
    [Activity(Label = "StatsActivity")]
    public class StatsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.StatsLayout);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            int curwins = prefs.GetInt(MatchupActivity.CURWINS, 0);
            int curloss = prefs.GetInt(MatchupActivity.CURLOSS, 0);
            int curtie = prefs.GetInt(MatchupActivity.CURTIE, 0);
            int curuserscore = prefs.GetInt(MatchupActivity.CURUSERSCORE, 0);
            int curoppscore = prefs.GetInt(MatchupActivity.CUROPPSCORE, 0);
            int allwins = prefs.GetInt(MatchupActivity.ALLWINS, 0);
            int allloss = prefs.GetInt(MatchupActivity.ALLLOSS, 0);
            int alltie = prefs.GetInt(MatchupActivity.ALLTIE, 0);
            int alluserscore = prefs.GetInt(MatchupActivity.ALLUSERSCORE, 0);
            int alloppscore = prefs.GetInt(MatchupActivity.ALLOPPSCORE, 0);
            int championships = prefs.GetInt(ChampionActivity.CHAMPIONSHIPS, 0);

            TextView curWins = FindViewById<TextView>(Resource.Id.curWin);
            TextView curLoss = FindViewById<TextView>(Resource.Id.curLoss);
            TextView curTie = FindViewById<TextView>(Resource.Id.curTie);
            TextView curUserScore = FindViewById<TextView>(Resource.Id.curUserScore);
            TextView curOppScore = FindViewById<TextView>(Resource.Id.curOppScore);
            TextView allWins = FindViewById<TextView>(Resource.Id.alltimeWin);
            TextView allLoss = FindViewById<TextView>(Resource.Id.alltimeLoss);
            TextView allTie = FindViewById<TextView>(Resource.Id.alltimeTie);
            TextView allUserScore = FindViewById<TextView>(Resource.Id.alltimeUserScore);
            TextView allOppScore = FindViewById<TextView>(Resource.Id.alltimeOppScore);
            TextView Championships = FindViewById<TextView>(Resource.Id.championships);

            curWins.Text = curwins.ToString();
            curLoss.Text = curloss.ToString();
            curTie.Text = curtie.ToString();
            curUserScore.Text = curuserscore.ToString();
            curOppScore.Text = curoppscore.ToString();
            allWins.Text = allwins.ToString();
            allLoss.Text = allloss.ToString();
            allTie.Text = alltie.ToString();
            allUserScore.Text = alluserscore.ToString();
            allOppScore.Text = alloppscore.ToString();
            Championships.Text = championships.ToString();
        }
    }
}