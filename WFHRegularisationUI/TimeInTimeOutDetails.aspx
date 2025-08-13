<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TimeInTimeOutDetails.aspx.cs" Inherits="WFHRegularisationUI.TimeInTimeOutDetails" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="head" ContentPlaceHolderID="cph_JS_PlaceHolder" runat="server">
	<script>

		window.onload = function () {
			flatpickr(".datepicker", {
				dateFormat: "d-m-Y",
				allowInput: true
			});
		};
	</script>
</asp:Content>

<asp:Content ID="MainPageContent" ContentPlaceHolderID="cph_MainContent" runat="server">
	<div class="alert alert-success alert-dismissible fade" runat="server" id="pageAlert" role="alert">
		<strong runat="server" id="AlertHeader">Success!</strong> <span runat="server" id="AlertMessage">Your data has been saved.</span>
		<button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
	</div>
	<div class="card shadow mb-4" id="divMain" runat="server">
		<div class="card-header d-flex justify-content-between align-items-center bg-primary text-white" style="cursor: move;">
			<div class="card-title">
				<i class="fa fa-file"></i>
				<span class="ms-2">Employee Time-In / Time-Out Details</span>
			</div>
			<button class="btn btn-light btn-sm toggle-collapse" type="button" data-bs-toggle="collapse" data-bs-target="#viewTimeInOutDetailsBody" aria-expanded="true" aria-controls="viewTimeInOutDetailsBody">
				<i class="fa fa-minus"></i>
			</button>
		</div>
		<!-- /.box-header -->
		<div class="card-body collapse show" id="viewTimeInOutDetailsBody">
			<div class="row">
				<div class="col-md-3">
					<div class="form-group">
						<label id="lblStartDate">Start date (DD-MM-YYYY)</label>
						<div class="input-group">
							<span class="input-group-addon for-datetime"><i class="fa fa-calendar"></i></span>
							<asp:TextBox ID="txtStartDate" MaxLength="10" runat="server" type="text" CssClass="form-control datepicker"></asp:TextBox>
						</div>
						<span id="startDateError" style="color: red; display: none;">Invalid date format.</span>
					</div>
				</div>
				<div class="col-md-3">
					<div class="form-group">
							<label ID="lblEndDate">End date (DD-MM-YYYY)</label>
						<div class="input-group">
							<span class="input-group-addon for-datetime"><i class="fa fa-calendar"></i></span>
							<asp:TextBox ID="txtEndDate" MaxLength="10" runat="server" type="text" CssClass="form-control datepicker"></asp:TextBox>
						</div>
						<span id="endDateError" style="color: red; display: none;">Invalid date format.</span>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-12">
					<div class="form-group">
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-12">
					<div class="form-group">

						<asp:GridView AutoGenerateColumns="false" AllowPaging="true" EmptyDataText="No time entry found."
							GridLines="Both" ID="gvTimeInTimeOutDetails" PageSize="10" runat="server"
							CssClass="table table-bordered table-striped table-fit"
							OnRowDataBound="gvTimeInTimeOutDetails_RowDataBound"
							OnPageIndexChanging="gvTimeInTimeOutDetails_PageIndexChanging">
							<Columns>
								<asp:TemplateField HeaderText="DASID">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblDASID" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="EmployeeName">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblEmployeeName" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="SAPNumber">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblSAPNumber" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="ServiceLine">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblServiceLine" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="OrgUnitnumber">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblOrgUnitnumber" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Company">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblCompany" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Location">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblLocation" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Time in (YYYY-MM-DD HH:MM)">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblTimeIn" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Time out (YYYY-MM-DD HH:MM)">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblTimeout" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Total time (HH:MM)">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblTotalTime" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
							<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
								Position="Bottom" />
						</asp:GridView>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-3">
					<div class="form-group">
						<asp:Button ID="btnSearchTime"
							CssClass="btn btn-primary"
							runat="server" Text="Search time"
							OnClientClick="return validateForm();"
							OnClick="btnSearchTime_Click" />
					</div>
				</div>
				<div class="col-md-3">
					<div class="form-group">
						<asp:Button ID="btnExportToExcel" CssClass="btn btn-primary"
							runat="server" Text="Export To Excel"
							OnClientClick="return validateForm();"
							OnClick="btnExportToExcel_Click" />
					</div>
				</div>
				<div class="col-md-3">
					<div class="form-group">
						<asp:Button CausesValidation="false" ID="btnClose" CssClass="btn btn-primary" runat="server" Text="Close" OnClientClick="closeWindow();" />
					</div>
				</div>
			</div>
		</div>
	</div>


	<div class="card shadow mb-4" id="divArchived" runat="server">
		<div class="card-header d-flex justify-content-between align-items-center bg-primary text-white" style="cursor: move;">
			<div class="card-title">
				<i class="fa fa-file"></i>
				<span class="ms-2">Archived Employee Time-In / Time-Out Details</span>
			</div>
			<button class="btn btn-light btn-sm toggle-collapse" type="button" data-bs-toggle="collapse" data-bs-target="#viewTimeInOutDetailsBodyArchived" aria-expanded="true" aria-controls="viewTimeInOutDetailsBodyArchived">
				<i class="fa fa-minus"></i>
			</button>
		</div>
		<!-- /.box-header -->
		<div class="card-body collapse show" id="viewTimeInOutDetailsBodyArchived">
			<div class="row">
				<div class="col-md-3">
					<div class="form-group">
						<label ID="lblStartDateArchived">Start date (DD-MM-YYYY)</label>
						<div class="input-group">
							<span class="input-group-addon for-datetime"><i class="fa fa-calendar"></i></span>
							<asp:TextBox ID="txtStartDateArchived" MaxLength="10" runat="server" type="text" CssClass="form-control datepicker"></asp:TextBox>
						</div>
						<span id="startDateErrorArchived" style="color: red; display: none;">Invalid date format.</span>
					</div>
				</div>
				<div class="col-md-3">
					<div class="form-group">
						<label ID="lblEndDateArchived">End date (DD-MM-YYYY)</label>
						<div class="input-group">
							<span class="input-group-addon for-datetime"><i class="fa fa-calendar"></i></span>
							<asp:TextBox ID="txtEndDateArchived" MaxLength="10" runat="server" type="text" CssClass="form-control datepicker"></asp:TextBox>
						</div>
						<span id="endDateErrorArchived" style="color: red; display: none;">Invalid date format.</span>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-12">
					<div class="form-group">
						<asp:GridView AutoGenerateColumns="false" AllowPaging="true" EmptyDataText="No time entry found."
							GridLines="Both"
							CssClass="table table-bordered table-striped table-fit"
							ID="gvTimeInTimeOutDetailsArchived" PageSize="10" runat="server"
							OnRowDataBound="gvTimeInTimeOutDetailsArchived_RowDataBound"
							OnPageIndexChanging="gvTimeInTimeOutDetailsArchived_PageIndexChanging">
							<Columns>
								<asp:TemplateField HeaderText="DASID">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblDASIDArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="EmployeeName">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblEmployeeNameArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="SAPNumber">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblSAPNumberArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="ServiceLine">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblServiceLineArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="OrgUnitnumber">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblOrgUnitnumberArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Company">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblCompanyArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Location">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblLocationArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Time in (YYYY-MM-DD HH:MM)">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblTimeInArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Time out (YYYY-MM-DD HH:MM)">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblTimeoutArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Total time (HH:MM)">
									<HeaderStyle Font-Bold="true" />
									<ItemTemplate>
										<asp:Label ID="lblTotalTimeArchived" runat="server"></asp:Label>
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
							<PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast"
								Position="Bottom" />
						</asp:GridView>
					</div>
				</div>
			</div>
			<div class="row">
				<div class="col-md-3">
					<div class="form-group">
						<asp:Button ID="btnSearchTimeArchived" CssClass="btn btn-primary"
							runat="server" Text="Search time"
							OnClientClick="return validateFormArchived();"
							OnClick="btnSearchTimeArchived_Click" />
					</div>
				</div>
				<div class="col-md-3">
					<div class="form-group">
						<asp:Button ID="btnExportToExcelArchived" CssClass="btn btn-primary"
							runat="server" Text="Export To Excel"
							OnClientClick="return validateFormArchived();"
							OnClick="btnExportToExcelArchived_Click" />
					</div>
				</div>
				<div class="col-md-3">
					<div class="form-group">
						<asp:Button CausesValidation="false" ID="btnCloseArchived" CssClass="btn btn-primary" runat="server" Text="Close" OnClientClick="closeWindow();" />
					</div>
				</div>
			</div>
		</div>
	</div>
	<%--ADDED BY SHRADDHA: ARCHIVED DETAILS END--%>

	<%-- Modified by Shraddha on 23-Apr-2025.
		 Added a new form to fetch archived records. The existing form and the new one require separate validation.
		 The RequiredFieldValidator for one form was conflicting with the other, hence replaced it with custom JavaScript validation code.
	--%>
	<script type="text/javascript">
		function validateForm() {
			var result = true;
			// Regular expression to check if the date is in DD-MM-YYYY format
			var datePattern = /^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4}$/;

			//Validate Start Date
			var txtStartDate = document.getElementById('<%= txtStartDate.ClientID %>').value;
			var startDateError = document.getElementById('startDateError');

			// Check if the date is empty
			if (txtStartDate.trim() === "") {
				startDateError.textContent = "Start date is required.";
				startDateError.style.display = "block";
				result = false;
			}

			// Check if the date format is valid
			if (!datePattern.test(txtStartDate)) {
				startDateError.textContent = "Invalid date format.";
				startDateError.style.display = "block";
				result = false;
			}

			if (result)
				startDateError.style.display = "none"; // Hide error message if validation is passed


			//Validate End Date
			var txtEndDate = document.getElementById('<%= txtEndDate.ClientID %>').value;
			var endDateError = document.getElementById('endDateError');

			// Check if the date is empty
			if (txtEndDate.trim() === "") {
				endDateError.textContent = "End date is required.";
				endDateError.style.display = "block";
				result = false;
			}

			// Check if the date format is valid
			if (!datePattern.test(txtEndDate)) {
				endDateError.textContent = "Invalid date format.";
				endDateError.style.display = "block";
				result = false;
			}

			if (result)
				endDateError.style.display = "none"; // Hide error message if validation is passed


			return result;
		}


		function validateFormArchived() {
			var result = true;
			// Regular expression to check if the date is in DD-MM-YYYY format
			var datePatternArchived = /^(0[1-9]|[12][0-9]|3[01])-(0[1-9]|1[0-2])-\d{4}$/;

			//Validate Start Date
			var txtStartDateArchived = document.getElementById('<%= txtStartDateArchived.ClientID %>').value;
			var startDateErrorArchived = document.getElementById('startDateErrorArchived');

			// Check if the date is empty
			if (txtStartDateArchived.trim() === "") {
				startDateErrorArchived.textContent = "Start date is required.";
				startDateErrorArchived.style.display = "block";
				result = false;
			}

			// Check if the date format is valid
			if (!datePatternArchived.test(txtStartDateArchived)) {
				startDateErrorArchived.textContent = "Invalid date format.";
				startDateErrorArchived.style.display = "block";
				result = false;
			}

			if (result)
				startDateErrorArchived.style.display = "none"; // Hide error message if validation is passed


			//Validate End Date
			var txtEndDateArchived = document.getElementById('<%= txtEndDateArchived.ClientID %>').value;
			var endDateErrorArchived = document.getElementById('endDateErrorArchived');

			// Check if the date is empty
			if (txtEndDateArchived.trim() === "") {
				endDateErrorArchived.textContent = "End date is required.";
				endDateErrorArchived.style.display = "block";
				result = false;
			}

			// Check if the date format is valid
			if (!datePatternArchived.test(txtEndDateArchived)) {
				endDateErrorArchived.textContent = "Invalid date format.";
				endDateErrorArchived.style.display = "block";
				result = false;
			}

			if (result)
				endDateErrorArchived.style.display = "none"; // Hide error message if validation is passed

			return result;
		}
	</script>
</asp:Content>
