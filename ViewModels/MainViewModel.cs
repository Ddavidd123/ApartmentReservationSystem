using System;
using System.Collections.Generic;
using System.Text;

namespace Projekat.diagram
{
	public class MainViewModel
	{
		public ObservableCollection<Apartment> Apartments;
		public ObservableCollection<ApartmentOccupancyRecord> Records;
		public Apartment SelectedApartment;
		public ApartmentOccupancyRecord SelectedRecord;
		private UndoRedoService undoRedoService;
		private ApartmentService apartmentService;
		private OccupancyRecordService recordService;

		public void AddApartment()
		{
			throw new NotImplementedException();
		}

		public void UpdateApartment()
		{
			throw new NotImplementedException();
		}

		public void DeleteApartment()
		{
			throw new NotImplementedException();
		}

		public void AddRecord()
		{
			throw new NotImplementedException();
		}

		public void UpdateRecord()
		{
			throw new NotImplementedException();
		}

		public void DeleteRecord()
		{
			throw new NotImplementedException();
		}

		public void Undo()
		{
			throw new NotImplementedException();
		}

		public void Redo()
		{
			throw new NotImplementedException();
		}

		public void Search()
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		public void Load()
		{
			throw new NotImplementedException();
		}
	}
}
