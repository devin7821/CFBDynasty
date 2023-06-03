using Android.Content;
using Android.Database.Sqlite;

namespace CFB_Dynasty
{
    class TeamDatabaseHelper : SQLiteOpenHelper
    {
        private static string DBaseName = "CFBDbase";
        private static int DBaseVersion = 1;
        public TeamDatabaseHelper(Context context) : base(context, DBaseName,
            null, DBaseVersion) //the null is for advanced cursor features
        { }
        public override void OnCreate(SQLiteDatabase db) //mandatory
        {
            //SQLite expects _id as primary key name
            db.ExecSQL("CREATE TABLE TEAM (_id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "NAME TEXT, IMAGE_RESOURCE_ID INTEGER);");

            foreach (Team team in Team.m_teams)
            {
                InsertTeam(db, team.Name, team.ImageId);
            }
        }

        private static void InsertTeam(SQLiteDatabase db, string name, int id)
        {
            //ContentValues describe the data you want to put in the dbase
            ContentValues teamValue = new ContentValues();
            teamValue.Put("NAME", name);
            teamValue.Put("IMAGE_RESOURCE_ID", id);
            db.Insert("TEAM", null, teamValue); //null is column hack to insert an empty row
        }

        //OnUpgrade is mandatory
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {                           //newVersion passed in is 4
            if (oldVersion < 2)     //changes may be adding columns, rename\drop\add tables
            {                       //old version 1 runs code B, D
                                    //run code B         //old version 2 runs code D
                                    //old version 3 runs code C, D  
            }                       //old version 4 runs nothing
            if (oldVersion == 3)    //old version 6 same approach with OnDownGrade
            {
                //run code C
            }
            //run code D
        }
    }
}