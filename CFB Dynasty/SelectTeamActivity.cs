using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Database.Sqlite;
using Android.Database;
using Android.Util;

namespace CFB_Dynasty
{
    [Activity(Label = "SelectTeamActivity")]
    public class SelectTeamActivity : Activity
    {
        static string TAG = "SelectTeamActivity";// Tag for loggin SelectTeamActivity

        ListView list;
        private static SQLiteDatabase db;
        private static ICursor cursor;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.SelectTeamLayout);

            Log.Info(TAG, "OnCreate(Bundle) called");

            SQLiteOpenHelper teamDbHelper = new TeamDatabaseHelper(this);
            db = teamDbHelper.ReadableDatabase;

            cursor = db.Query("TEAM", new string[] { "_id", "NAME" },
                null, null, null, null, null);

            CursorAdapter cursorAdapter = new SimpleCursorAdapter(this,
                Android.Resource.Layout.SimpleListItem1, cursor,
                new string[] { "NAME" },
                new int[] { Android.Resource.Id.Text1 }, 0);

            list = FindViewById<ListView>(Resource.Id.listView1);
            list.Adapter = cursorAdapter;

            list.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Intent intent = new Intent(this, typeof(TeamActivity));
            intent.PutExtra(TeamActivity.EXTRA_TEAM_NUM, e.Position);
            StartActivity(intent);
        }
    }
}