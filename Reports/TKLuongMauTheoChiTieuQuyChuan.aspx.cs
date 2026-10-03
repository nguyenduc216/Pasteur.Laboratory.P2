using Pasteur.Core.Helper;
using Pasteur.Core.Manager;
using Pasteur.Core;
using Pasteur.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pasteur.Laboratory.P2.Common;

namespace Pasteur.Laboratory.P2.Reports
{
    public partial class TKLuongMauTheoChiTieuQuyChuan : BasePage
    {
        #region Properties

        public override string FUNCTION_CODE
        {
            get
            {
                return FunctionSystem.Code.Report_ChiTieu_CTQC;
            }
        }

        public override string FUNCTION_NAME
        {
            get
            {
                return FunctionSystem.Name.Report_ChiTieu_CTQC;
            }
        }


        protected string CurrentName
        {
            get
            {
                string rs = "Thống kê theo chỉ tiêu và quy chuẩn";

                return rs;
            }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            txtSearchNoiNhanMau.EnterSubmitClientID = btnSearch.ClientID;
            if (!IsPostBack)
            {
                dateFrom.DateValue = DateTimeHelper.AddFirstDayOfMonth(DateTime.Now);
                dateTo.DateValue = DateTime.Now;
                LoadData();
                LoadSource();
            }
        }

        private void LoadData()
        {
            List<TblCatLoaiMau> loaiMauCol = CategoryManager.GetLoaiMauALL(true);
            TblCatLoaiMau lall = new TblCatLoaiMau();
            lall.Id = -1;
            lall.Ten = "Tất cả";
            loaiMauCol.Insert(0, lall);
            cbLoaiMau.DataSource = loaiMauCol;
            cbLoaiMau.DataValueField = TblCatLoaiMau.Columns.Id;
            cbLoaiMau.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbLoaiMau.DataBind();

            List<TblHasTag> hasTagAll = CategoryManager.GetHasTagAll(true);
            TblHasTag empty = new TblHasTag();
            empty.Id = -1;
            empty.TenTag = "Tất cả";
            hasTagAll.Insert(0, empty);
            cbNoiLayMauGoiY.DataSource = hasTagAll;
            cbNoiLayMauGoiY.DataValueField = TblHasTag.Columns.Id;
            cbNoiLayMauGoiY.DataTextField = TblHasTag.Columns.TenTag;
            cbNoiLayMauGoiY.DataBind();

            List<TblCatQuyChuan> quyChuanCol = CategoryManager.GetQuyChuanAll(true);
            TblCatQuyChuan qall = new TblCatQuyChuan();
            qall.Id = -1;
            qall.Ten = "Tất cả";
            quyChuanCol.Insert(0, qall);
            cbQuyChuan.DataSource = quyChuanCol;
            cbQuyChuan.DataValueField = TblCatLoaiMau.Columns.Id;
            cbQuyChuan.DataTextField = TblCatLoaiMau.Columns.Ten;
            cbQuyChuan.DataBind();
        }

        private void LoadSource()
        {
            Session["Data_QuyChuan"] = new List<TblMauXetNghiemChiTieu>();
            //if (dateTo.DateValue == DateTime.MinValue)
            //{
            //    AddValidPromt(dateTo.ClientID, "Chọn ngày tới");
            //    dateTo.Focus();
            //}

            //if (dateFrom.DateValue == DateTime.MinValue)
            //{
            //    AddValidPromt(dateFrom.ClientID, "Chọn ngày bắt đầu");
            //    dateFrom.Focus();
            //}

            if (PageIsValid)
            {
                string addition = " AND 1 = 1 ";
                if (!string.IsNullOrEmpty(cbHoaSinh.SelectedValue))
                {
                    if (cbHoaSinh.SelectedValue == "Hoa")
                        addition = " and mxn.LuongMauHoa > 0 ";
                    else
                        addition = " and (mxn.LuongMau - mxn.LuongMauHoa) > 0 ";
                }
                if (!string.IsNullOrEmpty(txtSearchNoiNhanMau.Text))
                    addition += string.Format(" AND mxn.{0} like N'%{1}%' ", TblMauXetNghiem.Columns.NoiLayMau, txtSearchNoiNhanMau.Text);
                if (cbKQDG.SelectedValue == "1")
                    addition += string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau);
                else if (cbKQDG.SelectedValue == "0")
                    addition += string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau);
                if (cbLoaiMau.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDLoaiMau, cbLoaiMau.SelectedValue);
                if (cbNoiLayMauGoiY.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDNoiNhanMau, cbNoiLayMauGoiY.SelectedValue);
                if (!string.IsNullOrEmpty(hiddenID.Value))
                    addition += string.Format(" AND chiTieu.{0} = {1} ", TblChiTieuXetNghiem.Columns.Id, hiddenID.Value);
                if (cbQuyChuan.HasValue)
                    addition += string.Format(" AND mxn.{0} = {1} ", TblMauXetNghiem.Columns.IDQuyChuan, cbQuyChuan.SelectedValue);

                if (!string.IsNullOrEmpty(cbTinhTrangDuyet.SelectedValue))
                {
                    if (cbTinhTrangDuyet.SelectedValue == "2") addition += string.Format(" AND mxn.{0} is not null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau, cbTinhTrangDuyet.SelectedValue);
                    else if (cbTinhTrangDuyet.SelectedValue == "3") addition += string.Format(" AND mxn.{0} is null ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau, cbTinhTrangDuyet.SelectedValue);
                    else if (cbTinhTrangDuyet.SelectedValue == "4") addition += string.Format(" AND mxn.NgayHoaLy is not null ");
                    else if (cbTinhTrangDuyet.SelectedValue == "5") addition += string.Format(" AND mxn.NgayViSinh is not null ");
                    else addition += string.Format(" AND mxn.{0} = '{1}' ", TblMauXetNghiem.Columns.KetQuaDanhGiaMau, cbTinhTrangDuyet.SelectedValue);
                }

                if (dateFrom.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateFrom.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateTo.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayLayMau, dateTo.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayHoaLy, dateDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));
                if (dateVSDuyetNoiBoTu.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} >= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoTu.DateValue.ToString("yyyy-MM-dd 00:00:00"));
                if (dateVSDuyetNoiBoToi.DateValue != DateTime.MinValue) addition += string.Format(" AND mxn.{0} <= '{1}' ", TblMauXetNghiem.Columns.NgayViSinh, dateVSDuyetNoiBoToi.DateValue.ToString("yyyy-MM-dd 23:59:59"));

                List<TblMauXetNghiemChiTieu> chiTieuCol = XetNghiemManager.ReportGetChiTieu(dateFrom.DateValue, dateTo.DateValue, addition);

                List<TblMauXetNghiemChiTieu> chiTieuColResult = chiTieuCol.GroupBy(c => new
                {
                    c.TenLoaiMau,
                    c.IDChiTieu,
                    c.TenTaiCacKhoa,
                    c.TenQuyChuan,
                    c.DanhGiaMau
                })
                .Select(gcs => new TblMauXetNghiemChiTieu()
                {
                    TenLoaiMau = gcs.Key.TenLoaiMau,
                    TenTaiCacKhoa = gcs.Key.TenTaiCacKhoa,
                    IDChiTieu = gcs.Key.IDChiTieu,
                    TenQuyChuan = gcs.Key.TenQuyChuan,
                    DanhGiaMau = gcs.Key.DanhGiaMau,
                    Children = gcs.ToList()
                }).ToList<TblMauXetNghiemChiTieu>();

                Session["Data_QuyChuan"] = chiTieuCol;

                repData.DataSource = chiTieuColResult;
                repData.DataBind();
            }
            else
                ShowValidPromt();
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ltrTongMau.Text = ltrTongMauDat.Text = ltrTongMauKhongDat.Text = ltrTongKhongDanhGia.Text = "0";
            LoadSource();
        }

        protected string GeDiaChi(object id)
        {
            string rs = "";
            if (id != null && Session["Data_QuyChuan"] != null)
            {
                List<TblMauXetNghiemChiTieu> mauChiTieu = Session["Data_QuyChuan"] as List<TblMauXetNghiemChiTieu>;
                if (mauChiTieu != null && mauChiTieu.Count > 0)
                {
                    List<TblMauXetNghiemChiTieu> mauChiTieu2 = mauChiTieu.FindAll(p => p.IDChiTieu.HasValue && p.IDChiTieu.Value.ToString() == id.ToString());
                    if (mauChiTieu2.Count > 0)
                    {
                        List<TblMauXetNghiemChiTieu> chiTieuColResult = mauChiTieu2.GroupBy(c => new
                        {
                            c.NoiLayMau
                        })
                        .Select(gcs => new TblMauXetNghiemChiTieu()
                        {
                            NoiLayMau = gcs.Key.NoiLayMau,
                            Children = gcs.ToList()
                        }).ToList<TblMauXetNghiemChiTieu>().FindAll(p => !string.IsNullOrEmpty(p.NoiLayMau) && p.NoiLayMau.Trim() != "/");
                        if (chiTieuColResult.Count > 0)
                        {
                            rs = string.Format("<ul class='ul-list-data' onclick='showList({0})>'", id.ToString());
                            foreach (TblMauXetNghiemChiTieu mau in chiTieuColResult)
                            {
                                rs += string.Format("<li>+ {0}</li>", mau.NoiLayMau);
                            }
                            rs += "</ul>";
                        }
                    }
                }
            }
            return rs;
        }

        protected string GeDiaChi2(object id)
        {
            string rs = "";
            if (id != null && Session["Data_QuyChuan"] != null)
            {
                List<TblMauXetNghiemChiTieu> mauChiTieu = Session["Data_QuyChuan"] as List<TblMauXetNghiemChiTieu>;
                if (mauChiTieu != null && mauChiTieu.Count > 0)
                {
                    List<TblMauXetNghiemChiTieu> mauChiTieu2 = mauChiTieu.FindAll(p => p.IDChiTieu.HasValue && p.IDChiTieu.Value.ToString() == id.ToString());
                    if (mauChiTieu2.Count > 0)
                    {
                        List<TblMauXetNghiemChiTieu> chiTieuColResult = mauChiTieu2.GroupBy(c => new
                        {
                            c.NoiLayMau2
                        })
                        .Select(gcs => new TblMauXetNghiemChiTieu()
                        {
                            NoiLayMau2 = gcs.Key.NoiLayMau2,
                            Children = gcs.ToList()
                        }).ToList<TblMauXetNghiemChiTieu>().FindAll(p => !string.IsNullOrEmpty(p.NoiLayMau2) && p.NoiLayMau2.Trim() != "/");
                        if (chiTieuColResult.Count > 0)
                        {
                            rs = string.Format("<ul class='ul-list-data' onclick='showList({0})>'", id.ToString());
                            foreach (TblMauXetNghiemChiTieu mau in chiTieuColResult)
                            {
                                rs += string.Format("<li>+ {0}</li>", mau.NoiLayMau2);
                            }
                            rs += "</ul>";
                        }
                    }
                }
            }
            return rs;
        }

        int tongMau = 0, tongKhongDat = 0, tongDat = 0, tongKhongDanhGia = 0;
        protected string TinhTong(object data, string code)
        {
            string rs = "";
            if (data != null && data is TblMauXetNghiemChiTieu)
            {
                TblMauXetNghiemChiTieu mauXN = data as TblMauXetNghiemChiTieu;
                if (code == "tm")
                {
                    rs = mauXN.Children.Count.ToString("N0");
                    tongMau += mauXN.Children.Count;
                    ltrTongMau.Text = tongMau.ToString("N0");
                }
                else if (code == "1")
                {
                    List<TblMauXetNghiemChiTieu> mau = mauXN.Children.FindAll(p => p.DanhGiaMau.HasValue && p.DanhGiaMau.Value && p.TuDanhGia.HasValue && p.TuDanhGia.Value);
                    rs = mau.Count.ToString("N0");
                    tongDat += mau.Count;
                    ltrTongMauDat.Text = tongDat.ToString("N0");

                }
                else if (code == "2")
                {
                    List<TblMauXetNghiemChiTieu> mau = mauXN.Children.FindAll(p => p.DanhGiaMau.HasValue && p.DanhGiaMau.Value && p.TuDanhGia.HasValue && !p.TuDanhGia.Value);
                    rs = mau.Count.ToString("N0");
                    tongKhongDat += mau.Count;
                    ltrTongMauKhongDat.Text = tongKhongDat.ToString("N0");
                }
                else if (code == "3")
                {
                    List<TblMauXetNghiemChiTieu> mau = mauXN.Children.FindAll(p => !p.DanhGiaMau.HasValue || (p.DanhGiaMau.HasValue && !p.DanhGiaMau.Value));
                    rs = mau.Count.ToString("N0");
                    tongKhongDanhGia += mau.Count;
                    ltrTongKhongDanhGia.Text = tongKhongDanhGia.ToString("N0");
                }
            }
            return rs;
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            RunScript("ExportExcel", string.Format("'tblData','Report_LuongMauTheoChiTieuQuyChuan_{0}.xls'", DateTime.Now.ToString("ddMMyyyy")));
        }
    }
}