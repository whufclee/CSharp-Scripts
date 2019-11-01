using System;
using UIKit;
using TrainingRooms;
using System.Collections.Generic;
using Foundation;

namespace trainingrooms.ios
{
    public class RoomsDatasource : UITableViewSource
    {
        private List<TrainingRoom> _rooms;

        public RoomsDatasource(List<TrainingRoom> rooms)
        {
            _rooms = rooms;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("roomCell");
            var room = _rooms[indexPath.Row];

            cell.TextLabel.Text = room.Name;
            cell.DetailTextLabel.Text = room.Location;
            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return _rooms.Count;
        }

        public TrainingRoom GetItem(int row)
        {
            return _rooms[row];
        }
    }
}
