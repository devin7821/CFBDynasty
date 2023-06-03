using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CFB_Dynasty
{
    class Team
    {
        private string m_name;
        public string Name
        {
            get { return m_name; }
        }

        private int m_imageId;
        public int ImageId
        {
            get { return m_imageId; }
        }

        public override string ToString()
        {
            return m_name;
        }

        public static Team[] m_teams =
        {
            new Team("Alabama Crimson Tide", Resource.Drawable.Crimson_Tide),
            new Team("Arkansas Razorbacks", Resource.Drawable.Razorbacks),
            new Team("Florida Gators", Resource.Drawable.Gators),
            new Team("Georgia Bulldogs", Resource.Drawable.Bulldogs),
            new Team("Kentucky Wildcats", Resource.Drawable.Wildcats),
            new Team("LSU Tigers", Resource.Drawable.Tigers),
            new Team("Oregon Ducks", Resource.Drawable.Ducks),
            new Team("Oregon State Beavers", Resource.Drawable.Beavers),
            new Team("Stanford Cardinal", Resource.Drawable.Cardinal),
            new Team("Tennessee Volunteers", Resource.Drawable.Volunteers),
            new Team("Texas A&M Aggies", Resource.Drawable.Aggies),
            new Team("UCLA Bruins", Resource.Drawable.Bruins),
            new Team("Utah Utes", Resource.Drawable.Utes),
            new Team("USC Trojans", Resource.Drawable.Trojans),
            new Team("Washington Huskies", Resource.Drawable.Huskies),
            new Team("Washington State Cougars", Resource.Drawable.Cougars)
        };

        public Team(string name, int imageId)
        {
            m_name = name; m_imageId = imageId;
        }
    }
}