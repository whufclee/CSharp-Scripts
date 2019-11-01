using System;

namespace TrainingRooms
{
    public class TrainingRoom
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
        public string Location
        {
            get;
            set;
        }

        public int NumberComputers
        {
            get;
            set;
        }


        public override string ToString()
        {
            return Name;
        }
    }
}
