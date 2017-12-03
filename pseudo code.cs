​// Automatic check-in

Position latestLocation;
Position ealLocation;
int schoolRadius = 100;

void DetermineCheckIn() {
	if (hasClasses && !checkedIn) {
		checkInButton.Text = "Check in";
	} else {
		checkInButton.Text = "Check out";
	}
}

void CheckInButtonClicked() {
	if (checkedIn) {
		CheckOut();
	} else {
		CheckIn();
	}
}

void CheckIn() {
	if (WithinSchool()) {
		try {
			api.CheckIn();
			checkedIn = true;
		}
		catch(Exception) {
			throw;
			Console.log("Could not check in. Check your connection and location settings.");
		}
	} else {
		Console.log("You are currently not on school grounds.");
	}
}

void CheckOut() {
	api.CheckOut();
	checkedIn = false;
}

bool WithinSchool() {
	if (latestLocation = ealLocation) {
		return true;
	} else {
		return false;
	}
}