using Android.App;
using Android.OS;
using AbsenceApp.Droid.Fragments;

using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace AbsenceApp.Droid
{
    [Activity(Label = "Absence", MainLauncher = true, LaunchMode = Android.Content.PM.LaunchMode.SingleTop)]
    public class MainActivity : AppCompatActivity
    {

        BottomNavigationView bottomNavigation;
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.main);
            //var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            //if (toolbar != null)
            //{
            //    SetSupportActionBar(toolbar);
            //    SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            //    SupportActionBar.SetHomeButtonEnabled(false);

            //}

            bottomNavigation = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation);


            bottomNavigation.NavigationItemSelected += BottomNavigation_NavigationItemSelected;

            LoadFragment(Resource.Id.menu_check_in);
        }

        private void BottomNavigation_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            LoadFragment(e.Item.ItemId);
        }

        void LoadFragment(int id)
        {
            Android.Support.V4.App.Fragment fragment = null;
            switch (id)
            {
                case Resource.Id.menu_check_in:
                    fragment = Check_in.NewInstance();
                    break;
                case Resource.Id.menu_absence:
                    fragment = Absence.NewInstance();
                    break;
                case Resource.Id.menu_history:
                    fragment = History.NewInstance();
                    break;
            }
            if (fragment == null)
                return;

            SupportFragmentManager.BeginTransaction()
               .Replace(Resource.Id.content_frame, fragment)
               .Commit();
        }
    }
}

