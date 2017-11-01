using Android.App;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;

namespace AbsenceApp.Droid.Fragments
{
    class Absence : Android.Support.V4.App.Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public static Absence NewInstance()
        {
            var frag2 = new Absence { Arguments = new Bundle() };
            return frag2;
        }


        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignored = base.OnCreateView(inflater, container, savedInstanceState);
            return inflater.Inflate(Resource.Layout.check_in, null);
        }
    }
}