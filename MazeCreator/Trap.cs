﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MazeCreator
{
    class Trap
    {
        public enum Type
        {
            HoleTrap = 7,
            ConcealedTrap = 8,
            SecretPassage = 9,
        }

        Type type;

        public Trap(Type t)
        {
            type = t;
            App.GetLevel().CellMouseEnter += PlacingTrap;
            App.GetLevel().CellMouseDown += ConfirmPlaceTrap;
            App.GetLevel().KeyDown += CancelPlacing;
            App.GetLevel().Focus();
        }

        private void CancelPlacing(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                StopPlacing();
        }

        public void PlacingTrap(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.ColumnIndex;
            int y = e.RowIndex;

            try
            {
                Cell.ReloadAllInfo();
                Cell.Get(x, y).Style.BackColor = App.color[(int)type];
            }
            catch (ArgumentOutOfRangeException)
            { /* Mouse moved outside of grid */  }
        }

        public void ConfirmPlaceTrap(object sender, DataGridViewCellMouseEventArgs e)
        {
            int col = e.ColumnIndex;
            int row = e.RowIndex;

            if (e.Button == MouseButtons.Right)
            {
                StopPlacing(col, row);
                return;
            }
            else if (!Cell.PlacementAllowed(col, row))
                MessageBox.Show("You can't place a trap here.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else // Set value
            {
                Cell.SetValue((int)type, col, row);
                if (App.activeGrid > 0) // Place indicator
                    Cell.SetValue(10, col, row, App.activeGrid - 1);
                StopPlacing(col, row);
            }
        }

        private void StopPlacing(int x = -1, int y = -1)
        {
            App.GetLevel().CellMouseEnter -= PlacingTrap;
            App.GetLevel().CellMouseDown -= ConfirmPlaceTrap;
            App.GetLevel().KeyDown -= CancelPlacing;

            if (x == -1 || y == -1)
                Cell.ReloadAllInfo();
            else
                Cell.SetInfo(x, y);
        }
    }
}
