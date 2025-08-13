<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTimeInTimeOut.aspx.cs" Inherits="WFHRegularisationUI.AddTimeInTimeOut" MasterPageFile="~/Site1.master" %>

<asp:Content ID="head" ContentPlaceHolderID="cph_JS_PlaceHolder" runat="server">
	
</asp:Content>

<asp:Content ID="MainPageContent" ContentPlaceHolderID="cph_MainContent" runat="server">

	<div class="alert alert-success alert-dismissible fade" runat="server" id="pageAlert" role="alert">
		<strong runat="server" id="AlertHeader">Success!</strong> <span runat="server" id="AlertMessage">Your data has been saved.</span>
		<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
	</div>


	<div class="card shadow mb-4" id="divMain" runat="server">
		<div class="card-header d-flex justify-content-between align-items-center bg-primary text-white" style="cursor: move;">
			<div class="card-title">
				<i class="fa fa-clock"></i>
				<span class="ms-2">Record Time in / Time out</span>
			</div>
			<button class="btn btn-light btn-sm toggle-collapse" type="button" data-bs-toggle="collapse" data-bs-target="#addTimeInOutDetailsBody" aria-expanded="true" aria-controls="addTimeInOutDetailsBody">
				<i class="fa fa-minus"></i>
			</button>
		</div>
		<!-- /.box-header -->
		<div class="card-body collapse show" id="addTimeInOutDetailsBody">
			<div class="row">
				<div class="col-md-6">
					<div class="form-group">
						<h4>
							<asp:Label ID="lblChromeMessage" Font-Size="Large" Font-Bold="true" ForeColor="Red" runat="server" Text="Please use Chrome for this application."></asp:Label></h4>
					</div>
				</div>
				<div class="col-md-6">
					<div class="form-group">
						<h4>
							<asp:Label Font-Bold="true" ForeColor="Black" ID="lblForHelpDocument" Font-Size="Large" runat="server" Text="For help document"></asp:Label>
							<asp:HyperLink Font-Bold="true" ForeColor="Blue" Font-size="Large" Font-Underline="true" NavigateUrl="~/Doc/Work_from_Home_Time_Entry_User_Instructions-2020-04-13.pdf" ID="hlnkHelp" runat="server" Target="_blank" Text="click here"></asp:HyperLink>
						</h4>
					</div>
				</div>
				<div class="row">
					<div class="col-md-12">
						<div class="form-group">
							<asp:Label ID="lblMessageO" runat="server"></asp:Label>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtName">Name:</label>
							<asp:TextBox ID="txtName" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtCompany">Company:</label>
							<asp:TextBox ID="txtCompany" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtDASID">DASID:</label>
							<asp:TextBox ID="txtDASID" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtSAPNumber">SAP Number:</label>
							<asp:TextBox ID="txtSAPNumber" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtLocation">Location:</label>
							<asp:TextBox ID="txtLocation" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtOrgUnit">Org Unit:</label>
							<asp:TextBox ID="txtOrgUnit" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-6">
						<div class="form-group">
							<label for="txtServiceLine">Service Line:</label>
							<asp:TextBox ID="txtServiceLine" runat="server" Text="" CssClass="form-control" Enabled="false"></asp:TextBox>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-12">
						<div class="form-group">
							<asp:CheckBox AutoPostBack="true" Checked="false" Font-Bold="true" ID="chkIsInShift" runat="server" Text="I am in shift / worked past midnight &nbsp;" TextAlign="Left" OnCheckedChanged="chkIsInShift_CheckedChanged" />
							<br />
							<asp:Label Font-Bold="true" ForeColor="Red" ID="lblIsInShift" runat="server" Text="System has detected difference in, ''TIME IN DATE'' and ''TIME OUT DATE''. You might be in Shift / Worked past midnight. If No, please uncheck the above check box and save the time for the day. If Yes, please confirm that the Shift / Worked past midnight check box is checked and then save the time for the day." Visible="false"></asp:Label>
							<br />
							<asp:Label Font-Bold="true" ForeColor="Red" ID="lblNotInShift" runat="server" Text="System has detected single day ''TIME IN DATE'' and ''TIME OUT DATE''. This means your ''TIME IN'' and ''TIME OUT'' are on the same day. The time shown will be your time for the day." Visible="false"></asp:Label>
							<br />
							<asp:Label Font-Bold="true" ForeColor="Red" ID="lblNoTimeInOnlyTimeOut" runat="server" Text="System has detected only ''TIME OUT''. This means you might have not saved your ''TIME IN''. The time shown will be your time for the day. However you can save your ''TIME IN'' tomorrow as usual." Visible="false"></asp:Label>
						</div>
					</div>
				</div>
				<div id="divDate" class="row" runat="server">
					<div class="col-md-3">
						<div class="form-group">

							<%--<asp:Label ID="lblTimeInDate" runat="server" Text="Time in Date (DD-MM-YYYY)"></asp:Label>--%>
							<label ID="lblTimeInDate" >Time in Date (DD-MM-YYYY):</label>
							<div class="input-group">
								<span class="input-group-addon for-datetime"><i class="fa fa-calendar"></i></span>
								<asp:TextBox ID="txtTimeInDate" MaxLength="10" runat="server" type="text" CssClass="form-control datepicker" Enabled="false"></asp:TextBox>
							</div>
						</div>
					</div>
					<div class="col-md-3">
						<div class="form-group">
							<label ID="lblTimeOutDate">Time out Date (DD-MM-YYYY):</label>
							<div class="input-group">
								<span class="input-group-addon for-datetime"><i class="fa fa-calendar"></i></span>
								<asp:TextBox ID="txtTimeOutDate" MaxLength="10" runat="server" type="text" CssClass="form-control datepicker" Enabled="false"></asp:TextBox>
							</div>
						</div>
					</div>
				</div>
				<div id="divTimeInTimeOut" class="row" runat="server">
					<div class="col-md-3">
						<div class="form-group">
							<label ID="lblTimeInHeader">Time in (24 hr HH:MM format.)</label>
							<div class="input-group">
								<span class="input-group-addon for-datetime"><i class="fa fa-clock"></i></span>
								<asp:TextBox ID="txtTimeIn" MaxLength="5" runat="server" type="text" CssClass="form-control"></asp:TextBox>
							</div>
						</div>
					</div>
					<div class="col-md-3">
						<div class="form-group">
							<label ID="lblTimeOutHeader">Time out (24 hr HH:MM format.)</label>
							<div class="input-group">
								<span class="input-group-addon for-datetime"><i class="fa fa-clock"></i></span>
								<asp:TextBox ID="txtTimeOut" MaxLength="5" runat="server" type="text" CssClass="form-control"></asp:TextBox>
							</div>
						</div>
					</div>
				</div>
				<div class="row">
					<div class="col-md-3">
						<div class="form-group">
							<asp:Button ID="btnSaveTimeIn" CssClass="btn btn-primary" runat="server" Text="Save time in" OnClick="btnSaveTimeIn_Click" />
						</div>
					</div>
					<div class="col-md-3">
						<div class="form-group">
							<asp:Button ID="btnSaveTimeOut" CssClass="btn btn-primary" runat="server" Text="Save time out" OnClick="btnSaveTimeOut_Click" />
						</div>
					</div>
					<div class="col-md-3">
						<div class="form-group">
							<asp:Button CausesValidation="false" ID="btnViewTimeInTimeOutDetails" CssClass="btn btn-primary" runat="server" Text="View time in time out details" OnClick="btnViewTimeInTimeOutDetails_Click" />
						</div>
					</div>
					<div class="col-md-3">
						<div class="form-group">
							<asp:Button ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClientClick="javascript:window.close();;" />
						</div>
					</div>
				</div>
				<div id="divWFHEmployeeTimeID" class="col-md-6" runat="server">
					<div class="form-group">

						<asp:Label Font-Bold="true" ForeColor="Red" ID="lblWFHEmployeeTimeID" runat="server" Text="0" Visible="false"></asp:Label>

					</div>
				</div>
				<div id="divTimeInTimeOutType" class="col-md-6" runat="server">
					<div class="form-group">

						<asp:Label ID="lblTimeInTimeOutType" runat="server" Text="" Visible="false"></asp:Label>

					</div>
				</div>
				<div id="divWFMEmployeeTimeInTimeOut" class="col-md-6" runat="server">
					<div class="form-group">

						<asp:Label ID="lblWFMEmployeeTimeInTimeOutID" runat="server" Text="0" Visible="false"></asp:Label>

					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>
