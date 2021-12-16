using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

// DataGridViewCheckBoxLabelCell - DataGridViewCheckBoxCell with label :-)
// Usage:
// 1. In DataGridView's property page, select DataGridViewCheckBoxLabelColumn
//    for row class.
//
// 2. Cast cell to DataGridViewCheckBoxCell, use LabelText property for setting
//    label text.

// TODO: Namespace is 'AdvancedDataGridView', for using with Mr. Mark Rideout's
// TreeGridView. Please collect it.
namespace ICApiAddin.icPowerApps
{
    public class DataGridViewCheckBoxLabelCell : DataGridViewCheckBoxCell
    {
        private string _text = string.Empty;
        private Point _mouseLocation;
        private PushButtonState checkState = PushButtonState.Normal;
        private Point _checkboxLocation;
        private Rectangle _checkboxHitArea;
        private Rectangle _textRectangle;
        private bool _focused = false;

        public DataGridViewCheckBoxLabelCell()
        {
        }
        public DataGridViewCheckBoxLabelCell(string defaultLabelText)
        {
            LabelText = defaultLabelText;
        }
        public override object Clone()
        {
            DataGridViewCheckBoxLabelCell c = (DataGridViewCheckBoxLabelCell)base.Clone();

            c._text = this._text;

            return c;
        }

        public string LabelText
        {
            get { return _text; }
            set { _text = value; }
        }

        protected override Rectangle GetContentBounds(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex)
        {
            return _checkboxHitArea;
        }

        protected override void OnKeyDown(KeyEventArgs e, int rowIndex)
        {
            base.OnKeyDown(e, rowIndex);
            if ((e.KeyData & Keys.Space) == Keys.Space)
            {
                checkState = PushButtonState.Pressed;
                DataGridView.InvalidateCell(this);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e, int rowIndex)
        {
            base.OnKeyUp(e, rowIndex);
            if ((e.KeyData & Keys.Space) == Keys.Space)
            {
                checkState = PushButtonState.Normal;
                DataGridView.InvalidateCell(this);
            }
        }

        protected override void OnEnter(int rowIndex, bool throughMouseClick)
        {
            _focused = true;
        }

        protected override void OnLeave(int rowIndex, bool throughMouseClick)
        {
            _focused = false;
            if (checkState != PushButtonState.Normal)
            {
                checkState = PushButtonState.Normal;
                DataGridView.InvalidateCell(this);
            }
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                if (_checkboxHitArea.Contains(e.Location))
                {
                    checkState = PushButtonState.Pressed;
                    DataGridView.InvalidateCell(this);
                }
            }
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            if (checkState != PushButtonState.Normal)
            {
                checkState = PushButtonState.Normal;
                DataGridView.InvalidateCell(this);
            }
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            _mouseLocation = e.Location;
            if (_checkboxHitArea.Contains(_mouseLocation))
            {
                if (checkState != PushButtonState.Hot && checkState != PushButtonState.Pressed)
                {
                    checkState = PushButtonState.Hot;
                    DataGridView.InvalidateCell(this);
                }
            }
            else
            {
                if (checkState != PushButtonState.Normal)
                {
                    checkState = PushButtonState.Normal;
                    DataGridView.InvalidateCell(this);
                }
            }
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                checkState = PushButtonState.Normal;
                DataGridView.InvalidateCell(this);
            }
        }

        protected override Size GetPreferredSize(
            Graphics graphics,
            DataGridViewCellStyle cellStyle,
            int rowIndex,
            Size constraintSize)
        {
            Size size = base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);

            Size textSize = Size.Ceiling(graphics.MeasureString(_text, cellStyle.Font));
            if (textSize.Width > 0)
            {
                size.Width += 1 + textSize.Width;
            }
            if (textSize.Height > size.Height)
            {
                size.Height = textSize.Height + cellStyle.Padding.Vertical;
            }
            return size;
        }

        protected virtual void CalcGeometory(
            Graphics graphics,
            Rectangle cellBounds,
            DataGridViewCellStyle cellStyle,
            int rowIndex)
        {
            Size size = GetPreferredSize(graphics, cellStyle, rowIndex, new Size(0, 0));
            Point location = cellBounds.Location;
            location.X += cellStyle.Padding.Left;
            location.Y += cellStyle.Padding.Top;
            Size offset = new Size(0, 0);
            int xs = cellBounds.Width - size.Width;
            int ys = cellBounds.Height - size.Height;
            switch (cellStyle.Alignment)
            {
                case DataGridViewContentAlignment.BottomCenter:
                    offset.Width = xs / 2;
                    offset.Height = ys;
                    break;
                case DataGridViewContentAlignment.BottomLeft:
                    offset.Width = 0;
                    offset.Height = ys;
                    break;
                case DataGridViewContentAlignment.BottomRight:
                    offset.Width = xs;
                    offset.Height = ys;
                    break;
                case DataGridViewContentAlignment.MiddleCenter:
                    offset.Width = xs / 2;
                    offset.Height = ys / 2;
                    break;
                case DataGridViewContentAlignment.MiddleLeft:
                    offset.Width = 0;
                    offset.Height = ys / 2;
                    break;
                case DataGridViewContentAlignment.MiddleRight:
                    offset.Width = xs;
                    offset.Height = ys / 2;
                    break;
                case DataGridViewContentAlignment.TopCenter:
                    offset.Width = xs / 2;
                    offset.Height = 0;
                    break;
                case DataGridViewContentAlignment.TopLeft:
                    offset.Width = 0;
                    offset.Height = 0;
                    break;
                case DataGridViewContentAlignment.TopRight:
                    offset.Width = xs;
                    offset.Height = 0;
                    break;
            }
            if (offset.Width < 0)
            {
                offset.Width = 0;
            }
            if (offset.Height < 0)
            {
                offset.Height = 0;
            }
            location += offset;

            _checkboxLocation = location;
            Size sizeCheckBox = CheckBoxRenderer.GetGlyphSize(graphics, CheckBoxState.CheckedNormal);
            int checkY = (cellBounds.Height - sizeCheckBox.Height) / 2;
            if (checkY < 0)
            {
                checkY = 0;
            }
            _checkboxLocation.Offset(0, checkY);
            _checkboxHitArea = new Rectangle(location, sizeCheckBox);
            _checkboxHitArea.Offset(-cellBounds.Left, -cellBounds.Top);
            _checkboxHitArea.Width = size.Width;
            _textRectangle = new Rectangle(
                location.X + sizeCheckBox.Width + 1,
                location.Y,
                size.Width - cellStyle.Padding.Horizontal - sizeCheckBox.Width - 1,
                size.Height);
        }

        protected override void Paint(
            Graphics graphics,
            Rectangle clipBounds,
            Rectangle cellBounds,
            int rowIndex,
            DataGridViewElementStates elementState,
            object value,
            object formattedValue,
            string errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            //base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            CalcGeometory(graphics, cellBounds, cellStyle, rowIndex);

            // Draw the cell background, if specified.
            if ((paintParts & DataGridViewPaintParts.Background) ==
                DataGridViewPaintParts.Background)
            {

                SolidBrush cellBackground =
                    new SolidBrush(
                        ((elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                        ? cellStyle.SelectionBackColor
                        : cellStyle.BackColor);
                graphics.FillRectangle(cellBackground, cellBounds);
                cellBackground.Dispose();
            }

            // Draw the cell borders, if specified.
            if ((paintParts & DataGridViewPaintParts.Border) ==
                DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle,
                    advancedBorderStyle);
            }

            CheckBoxState state = CheckBoxState.CheckedNormal;
            object v = formattedValue;
            CheckState s = CheckState.Unchecked;
            if (v is CheckState)
            {
                s = (CheckState)v;
            }
            else if (v is bool)
            {
                if ((bool)v)
                {
                    s = CheckState.Checked;
                }
                else
                {
                    s = CheckState.Unchecked;
                }
            }
            switch (s)
            {
                case CheckState.Unchecked:
                    state = (CheckBoxState)checkState;
                    break;
                case CheckState.Checked:
                    state = (CheckBoxState)((int)checkState + 4);
                    break;
                case CheckState.Indeterminate:
                    state = (CheckBoxState)((int)checkState + 8);
                    break;
            }

            CheckBoxRenderer.DrawCheckBox(
                graphics,
                _checkboxLocation,
                state);
            {
                SolidBrush cellText =
                    new SolidBrush(
                        ((elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                        ? cellStyle.SelectionForeColor
                        : cellStyle.ForeColor);

                StringFormat sf = new StringFormat();
                sf.Alignment = StringAlignment.Center;
                sf.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;
                sf.LineAlignment = StringAlignment.Center;

                graphics.DrawString(
                    _text,
                    cellStyle.Font,
                    cellText,
                    _textRectangle,
                    sf);

                cellText.Dispose();
            }

            // Draw the focus rectangle, if specified.
            if ((paintParts & DataGridViewPaintParts.Focus) ==
                DataGridViewPaintParts.Focus && _focused)
            {
                Rectangle rect = cellBounds;
                rect.Offset(cellStyle.Padding.Left, cellStyle.Padding.Top);
                rect.Width -= cellStyle.Padding.Horizontal;
                rect.Height -= cellStyle.Padding.Vertical;

                if ((elementState & DataGridViewElementStates.Selected) == DataGridViewElementStates.Selected)
                {
                    ControlPaint.DrawFocusRectangle(graphics, rect, cellStyle.SelectionForeColor, cellStyle.SelectionBackColor);
                }
                else
                {
                    ControlPaint.DrawFocusRectangle(graphics, rect, cellStyle.ForeColor, cellStyle.BackColor);
                }
            }
        }
    }

    public class DataGridViewCheckBoxLabelColumn : DataGridViewCheckBoxColumn
    {
        public DataGridViewCheckBoxLabelColumn()
        {
            this.CellTemplate = new DataGridViewCheckBoxLabelCell();
        }
        public DataGridViewCheckBoxLabelColumn(string defaultLabelText)
        {
            this.CellTemplate = new DataGridViewCheckBoxLabelCell(defaultLabelText);
        }
    }
}