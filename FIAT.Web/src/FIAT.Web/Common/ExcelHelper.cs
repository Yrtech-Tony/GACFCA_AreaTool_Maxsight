using Npoi.Core.SS.UserModel;
using Npoi.Core.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIAT.Web.Common
{
    public class ExcelHelper
    {
        IWorkbook workbook = new XSSFWorkbook();

        public ICell createCell()
        {
            return null;
        }

        public IFont FONT_B
        {
            get{
                IFont font_b = workbook.CreateFont();
                font_b.FontName = "微软雅黑";
                font_b.FontHeightInPoints = 10;
                return font_b;
            }
        }
        public ICellStyle Style1
        {
            get
            {
                var style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                byte[] rgb = new byte[3] { 116, 163, 210 };
                style1.SetFillForegroundColor(new XSSFColor(rgb));
                style1.FillPattern = FillPattern.SolidForeground;
                style1.Alignment = HorizontalAlignment.Center;
                style1.BorderLeft = BorderStyle.Thin;
                style1.BorderBottom = BorderStyle.Thin;
                style1.BorderRight = BorderStyle.Thin;
                style1.BorderTop = BorderStyle.Thin;
                style1.SetFont(FONT_B);

                return style1;
            }

        }
        public ICellStyle Style2
        {
            get
            {
                var style2 = workbook.CreateCellStyle();
                style2.Alignment = HorizontalAlignment.Left;
                style2.SetFont(FONT_B);

                return style2;
            }
        }
        public ICellStyle Style_b2
        {
            get
            {
                var style_b2 = workbook.CreateCellStyle();
                style_b2.WrapText = true;
                style_b2.VerticalAlignment = VerticalAlignment.Center;
                style_b2.SetFont(FONT_B);

                return style_b2;
            }
        }
        public ICellStyle Style3
        {
            get
            {
                var style3 = workbook.CreateCellStyle();
                style3.Alignment = HorizontalAlignment.Right;
                style3.SetFont(FONT_B);

                return style3;
            }
        }

        public ICellStyle Style4
        {
            get
            {
                var style4 = workbook.CreateCellStyle();
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;
                style4.SetFont(FONT_B);

                return style4;
            }
        }


       
    }
}
