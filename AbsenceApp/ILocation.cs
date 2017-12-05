using System;

namespace AbsenceApp {
    public interface ILocation {
        void StartListener();
        event EventHandler<ILocationEventArgs> locationObtained;
    }
    public interface ILocationEventArgs {
        double lat { get; set; }
        double lng { get; set; }
    }
}