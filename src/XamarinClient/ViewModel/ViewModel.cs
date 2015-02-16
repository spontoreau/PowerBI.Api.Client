using System;
using System.ComponentModel;

namespace XamarinClient
{
	/// <summary>
	/// View model.
	/// </summary>
	public abstract class ViewModel : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the property changed event.
		/// </summary>
		/// <param name="propertyName">Property name.</param>
		protected void OnPropertyChanged(string propertyName)
		{
			var tmp = PropertyChanged;

			if(tmp != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}

