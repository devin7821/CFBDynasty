using System;
using Android;
using Android.Content;
using Android.OS;

using Android.Views;
using Android.Widget;
using AndroidX.Core.View;
using AndroidX.Fragment.App;
using AndroidX.Preference;

namespace CFB_Dynasty
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class StatsFragClass : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.StatsLayout, container, false);
        }

        public override void OnStart()
        {
            base.OnStart();
            View view = View;
            if (view != null) 
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this.Activity);

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

                TextView curWins = (TextView)view.FindViewById<TextView>(Resource.Id.curWin);
                TextView curLoss = (TextView)view.FindViewById<TextView>(Resource.Id.curLoss);
                TextView curTie = (TextView)view.FindViewById<TextView>(Resource.Id.curTie);
                TextView curUserScore = (TextView)view.FindViewById<TextView>(Resource.Id.curUserScore);
                TextView curOppScore = (TextView)view.FindViewById<TextView>(Resource.Id.curOppScore);
                TextView allWins = (TextView)view.FindViewById<TextView>(Resource.Id.alltimeWin);
                TextView allLoss = (TextView)view.FindViewById<TextView>(Resource.Id.alltimeLoss);
                TextView allTie = (TextView)view.FindViewById<TextView>(Resource.Id.alltimeTie);
                TextView allUserScore = (TextView)view.FindViewById<TextView>(Resource.Id.alltimeUserScore);
                TextView allOppScore = (TextView)view.FindViewById<TextView>(Resource.Id.alltimeOppScore);
                TextView Championships = (TextView)view.FindViewById<TextView>(Resource.Id.championships);

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

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }
    }
}