using Android.App;
using Android.Content;
using Android.Database.Sqlite;
using Android.Database;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Preferences;

namespace CFB_Dynasty
{
    [Activity(Label = "TeamActivity")]
    public class TeamActivity : Activity
    {
        public static string EXTRA_TEAM_NUM = "extraTeam";
        public static string TEAM = "team";// Tag for selected team
        public static string IMG_NUM = "image";//Tag for image
        public static string DB_NUM = "dbNum";//Tag for team id in db

        Button selectButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.TeamLayout);

            selectButton = FindViewById<Button>(Resource.Id.select_team_button);

            int teamNum = Intent.GetIntExtra(TeamActivity.EXTRA_TEAM_NUM, 0);
            ++teamNum; //id starts at 1

            try
            {
                SQLiteOpenHelper teamHelper = new TeamDatabaseHelper(this);
                SQLiteDatabase db = teamHelper.ReadableDatabase;

                ICursor cursor = db.Query("TEAM",
                    new string[] { "NAME", "IMAGE_RESOURCE_ID" },
                    "_id = ?", new string[] { teamNum.ToString() }, null, null, null); //"_id = ?" where the contents equal mealNum

                if (cursor.MoveToFirst()) //must do
                {
                    ImageView photo = FindViewById<ImageView>(Resource.Id.photo);
                    photo.SetImageResource(cursor.GetInt(1)); //column 2

                    TextView name = FindViewById<TextView>(Resource.Id.name);
                    name.Text = cursor.GetString(0);
                }

                selectButton.Click += delegate
                {
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(this);
                    ISharedPreferencesEditor editor = prefs.Edit();

                    editor.PutBoolean(MainActivity.SAVE, true);
                    editor.PutString(TEAM, cursor.GetString(0));
                    editor.PutInt(IMG_NUM, cursor.GetInt(1));
                    editor.PutInt(DB_NUM, teamNum);
                    editor.Apply();

                    Intent intent = new Intent(this, typeof(MatchupActivity));
                    StartActivity(intent);
                };
            }
            catch (SQLException e)
            {
                Toast.MakeText(this, "TeamActivityDbase unavailable", ToastLength.Short).Show();
            }         
        }
    }
}