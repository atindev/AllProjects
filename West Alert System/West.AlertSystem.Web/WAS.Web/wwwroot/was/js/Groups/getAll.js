
var locationData = [], shiftData = [], departmentData = [], cityData = [], stateData = [], countryData = [], employeeTypeData = [], employeeGroupData = [],jobTitleData =[];
var SelectedSubscription = [];
$(document).ready(function () {
    SelectedSubscription = [];

    //default selection of hide groups
    $("#idHideDeletedGroups").prop("checked", true);

    //Initialize Select2 Elements
    $('.select2').select2();

    //Initialize Select2 Elements
    $('.select2bs4').select2({
        theme: 'bootstrap4'
    })

    getFilterData();

    $('.newGroupPopup').hide();

    $('.radio-last').click(function (e) {
        $('.newGroupPopup').hide();
        $('#idExistingPeopleCount').hide();
        if (e.currentTarget.getAttribute("for") == "idexistingGroupSelection") {
            $('#idExistingGroupPopup').show();
            $('#idExistingPeopleCount').show();
            clearExistingGroupSelection();
        }
        else
            $('#idNewGroupPopup').show();
    });

    $('#idsearchGroups').bind("enterKey", function (e) {
        var searchString = $('#idsearchGroups').val();
        searchGrid("GridGroups", searchString);
    });

    $('#idsearchGroups').keyup(function (e) {
        if (e.keyCode == 13 || $(this).val() == "") {
            $(this).trigger("enterKey");
        }
    });

    $(document).on('change', '#filtertype', function () {
        showLoader();
        var closestFilterType = $(this);
        var closestFilterValue = $(this).parent().parent().parent().find('.filterDdl');
        var divFilterValue = $(this).parent().parent().parent().find('#divFilterValue');

        hideAndShowFilterConditionElement($(this).closest('.classConditionList .row'), closestFilterType.val())

        //If filter type is Location, Shift and Department
        if (closestFilterType.val() != 3) {
            divFilterValue.find('.filterText').hide();
            divFilterValue.find('.filterDdl').show();

            $.ajax({
                url: '/was/group/GetFilterValues',
                type: 'GET',
                data: { FilterType: closestFilterType.val() },
                success: function (result) {
                    hideLoader();
                    closestFilterValue.empty();
                    closestFilterValue.append($("<option/>").val('0').text('Any'));
                    switch (result.FilterType) {
                        case 0:
                            $.each(result.Locations, function () {
                                closestFilterValue.append($("<option/>").val(this.Id).text(this.Name));
                            });
                        case 1:
                            $.each(result.Shifts, function () {
                                closestFilterValue.append($("<option/>").val(this.Id).text(this.Name));
                            });
                        default:
                    }
                },
                error: function (err) {
                    hideLoader();
                },
                complete: function (data) {
                    hideLoader();
                }
            });
        }
        //If filter type is free text like email
        else {
            divFilterValue.find('.filterText').show();
            divFilterValue.find('.filterDdl').hide();
        }
    });

    $('#addRow').click(function () {
        $('#template').find("#divRow").clone().appendTo($("#container"));
    });

    $(document).on('click', '#deleteRow', function () {
        this.closest("#divRow").remove();
    });

    $(document).on('change', '.filterDdl', function () {
        var divFilterValue = $(this).parent().parent().parent().find('.filterText');
        divFilterValue.val($(this).val())
    });

    function hideAndShowFilterConditionElement(element, value) {

        $($(element).find('select[name="FilterConditions"]').find("option")).removeClass('DN')
        $($(element).find('select[name="FilterConditions"]')).val("");

        if (value != 3) {
            $($(element).find('select[name="FilterConditions"]').find('option:contains("Contains")')).addClass('DN');
            $($(element).find('select[name="FilterConditions"]')).val("=");
        }

        else {
            $($(element).find('select[name="FilterConditions"]').find("option")).addClass('DN');
            $($(element).find('select[name="FilterConditions"]').find('option:contains("Contains")')).removeClass('DN');
            $($(element).find('select[name="FilterConditions"]')).val("Contains");
            $($(element).find('.inputFilter')).val("");
        }
    }

    function createGetPeopleRequest() {

        let peopleFilter = {}, AndOrCondition = [], FilterTypes = [], FilterConditions = [], FilterValues = [], filterValue = "", filterType = "";

        $('.classConditionList .row').each(function () {

            filterValue = "", filterType = "";
            filterType = $(this).find('select[name="FilterTypes"]').find("option:selected").val();
            if (filterType != undefined && filterType != "") {
                AndOrCondition.push($(this).find('select[name="AndOrCondition"]').find("option:selected").val());
                FilterTypes.push($(this).find('select[name="FilterTypes"]').find("option:selected").text());
                FilterConditions.push($(this).find('select[name="FilterConditions"]').find("option:selected").text());
                //checking for input text
                if ($($(this).find('.inputFilter')).css('display') == "none")
                    filterValue = $(this).find('select[name="FilterDdlValues"]').find("option:selected").val();
                else
                    filterValue = $($(this).find('.inputFilter')).val();
                FilterValues.push((filterValue == "-1" || filterValue == "0" || filterValue == "") ? null : filterValue);
            }

        });

        peopleFilter["AndOrCondition"] = AndOrCondition;
        peopleFilter["FilterTypes"] = FilterTypes;
        peopleFilter["FilterConditions"] = FilterConditions;
        peopleFilter["FilterValues"] = FilterValues;

        return peopleFilter;
    }

    $('#idButtonRunQuery').click(function () {
        showLoader();
        $('.addSubscriptionParent').hide();
        SelectedSubscription = [];
        setFilteredCount();

        var queryBuilderGrid = ej.base.getInstance(document.getElementById("querybuilder"), ej.querybuilder.QueryBuilder);
        var peopleFilter = queryBuilderGrid.getValidRules(queryBuilderGrid.rule);

        var grid = document.getElementById('GridFilterdSubscriptions').ej2_instances[0]; // Grid instance
        grid.pageSettings.currentPage = 1;

        if (peopleFilter.rules.length == 0) {
            grid.query = new ej.data.Query()
                .addParams('QueryBuilder', null);
        }
        else {
            grid.query = new ej.data.Query()
                .addParams('QueryBuilder', peopleFilter);
        }

        //resetting old checkbox selection
        if ($(".e-checkselectall")[0].checked || $(".customcssSelectAll .e-stop").length > 0 || $(".customcssSelectAll .e-check").length > 0) {
            $(".customcssSelectAll .e-checkselectall").parent().click();
            if ($(".customcssSelectAll .e-check").length > 0)
                $(".customcssSelectAll .e-checkselectall").parent().click();
        }
    });

    function getFilterData() {
        showLoader();
        $.ajax({
            url: '/was/group/GetFilterValues',
            type: 'GET',
            //data: { FilterType: filterType },
            success: function (result) {
                locationData = shiftData = departmentData = cityData = stateData = countryData = employeeTypeData = employeeGroupData = [];
                if (result != undefined || result != null) {
                    let obj = {};
                    obj["Id"] = 0;
                    obj["Name"] = "Any";

                    if (result.Locations != undefined || result.Locations != null) {
                        locationData = result.Locations;
                        locationData.unshift(obj);
                    }
                    if (result.Shifts != undefined || result.Shifts != null) {
                        shiftData = result.Shifts;
                        shiftData.unshift(obj);
                    }
                    if (result.Departments != undefined || result.Departments != null) {
                        departmentData = result.Departments;
                        departmentData.unshift(obj);
                    }
                    if (result.Cities != undefined || result.Cities != null) {
                        cityData = result.Cities;
                        cityData.unshift(obj);
                    }
                    if (result.States != undefined || result.States != null) {
                        stateData = result.States;
                        stateData.unshift(obj);
                    }
                    if (result.Countries != undefined || result.Countries != null) {
                        countryData = result.Countries;
                        countryData.unshift(obj);
                    }
                    if (result.EmployeeTypes != undefined || result.EmployeeTypes != null) {
                        employeeTypeData = result.EmployeeTypes;
                        employeeTypeData.unshift("Any");

                    }
                    if (result.EmployeeGroups != undefined || result.EmployeeGroups != null) {
                        employeeGroupData = result.EmployeeGroups;
                        employeeGroupData.unshift("Any");

                    }
                    if (result.JobTitles != undefined || result.JobTitles != null) {
                        jobTitleData = result.JobTitles;
                        jobTitleData.unshift("Any");

                    }
                }
                hideLoader();
            },
            error: function (err) {
                hideLoader();
            },           
            complete: function (data) {
                hideLoader();
            }
        });
    }

    $('#idExistingGroups').change(function (event) {
        let selectedElements = getGroupsSelected();
        appendExistingGroupCount();
    });

    $('#idAccessOnlyToOwner').change(function (event) {
        enableDisablePrivacyCheckbox();
    });

    $(document).on('keypress', '#querybuilder .e-rule-value input[type=text]', function (e) {
        var key = e.which;
        if (key == 13) {
            $("#idButtonRunQuery").click();
            return false;
        }
    });

    function getGroupsSelected() {
        let selectedGroups = [];
        $("#idExistingGroups").find("option:selected").each(function (index, element) {
            let value = $(element).attr("value");
            if (value != undefined && value != null && value != "")
                selectedGroups.push(value);
        });
        return selectedGroups;
    }

    $('#idGrpDeleteConfirm,#idGrpRestoreConfirm,#idSubscriptionDeleteConfirm').click(function (e) {
        showLoader();
    });

    $('#idHideDeletedGroups').change(function (event) {
       
        var grid = document.getElementById("GridGroups").ej2_instances[0];
        let hideGroups = $('#idHideDeletedGroups').prop('checked');
        showLoader();
 
        $.ajax({
            url: "/was/Group/ShowHideGroups",
            type: "POST",
            data: {
                hideDeletedGroups: hideGroups
            },
            success: function (data) {
                grid.dataSource = [];
                if (data != null && data.Groups.length > 0) {
                    grid.dataSource = [];
                    grid.dataSource = data.Groups;
                }
                hideLoader();
            },
            error: function () {
                grid.dataSource = [];
                hideLoader();
            }
        });

    });

});

function getDefaultSubscribers() {

    let peopleFilter = {};
    peopleFilter["PageIndex"] = 0;
    peopleFilter["RowCount"] = 3;
    peopleFilter["PageType"] = "complete";
    bindSubscriptionGrid(peopleFilter);

}

function bindSubscriptionGrid(peopleFilter) {

    showLoader();
    $('.addSubscriptionParent').hide();
    SelectedSubscription = [];
    setFilteredCount();
    var grid = document.getElementById("GridFilterdSubscriptions").ej2_instances[0];

    //resetting old checkbox selection
    if ($(".e-checkselectall")[0].checked || $(".customcssAuthor .e-stop").length > 0) {
        $(".customcssAuthor .e-checkselectall").parent().click();
    }

    var ajax = new ej.base.Ajax({
        url: "/was/group/GetPeople",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(peopleFilter),

    });
    ajax.send().then(function (data) {
        grid.dataSource = [];
        if (data != null && data.length > 0 && JSON.parse(data).Subscriptions.length > 0) {
            grid.dataSource = JSON.parse(data).Subscriptions;
            setFilteredCount(JSON.parse(data).Subscriptions.length);
        }
        else
            $('.addSubscriptionParent').hide();

        hideLoader();

    }).catch(function (xhr) {
        grid.dataSource = [];
        $('.addSubscriptionParent').hide();
        hideLoader();

    });
}

$(window).on('load', function () {
    if ($('#idTabOption').val() == "People")
        $('#custom-content-below-profile-tab-groups').click();
});

function getPagedData() {

    let groupFilter = "";
    groupFilter = $('#idsearchGroups').val();

    var grid = document.getElementById('GridGroups').ej2_instances[0]; // Grid instance 
    grid.pageSettings.currentPage = 1;
    grid.query = new ej.data.Query()
        .addParams('GroupFilter', groupFilter)

}

function clearGroupContent() {
    $('#modal-lg .modal-title').text("Create Group");
    $('#idNewGroupName').val("");
    $('#idNewGroupDescription').val("");
    $('#idGroupID').val("");
}

function updateGroup(event) {
    let target = event.target;
    if (target.getAttribute("value") == "" || target.getAttribute("value") == undefined || target.getAttribute("value") != "editGroup")
        return false;
    $('#idAddGroupToggle').click();
    $('#modal-lg .modal-title').text("Update Group");
    $('#idGroupID').val((event.data.Id == null || event.data.Id == "") ? "" : event.data.Id);
    $('#idNewGroupName').val((event.data.Name == null || event.data.Name == "") ? "" : event.data.Name);
    $('#idNewGroupDescription').val((event.data.Description == null || event.data.Description == "") ? "" : event.data.Description);
    if (event.data.IsPrivate == true) {
        $("#IsPrivateEnabled").prop("checked", true);
    }
    else {
        $("#IsPrivateEnabled").prop("checked", false);
    }
    if (event.data.IsAccessToAdmins == true) {
        $("#IsAdminAccessEnabled").prop("checked", true);
    } else {
        $("#IsAdminAccessEnabled").prop("checked", false);
    }
    //if ((event.data.IsPrivate == false) && (event.data.IsAccessToAdmins == false)) {
        enableDisableIsAdminCheckbox();
    //}
    $('#idGroupCreatedBy').val("");
}

function updateUserDetails() {
    populateUserDetailsforGrid($(".ad-user-grid"));
    populateUserPicturesforGrid($(".img-circle-grid"));
}

function searchGrid(element, text) {
    var gridObj = document.getElementById(element).ej2_instances[0];
    gridObj.search(text);
}

function hideandShowButtons(visibleButton) {
    $('.addButton').hide();
    $('#' + visibleButton).show();

    if (visibleButton == "idAddSubscriptionsToGroup")
        $("#idAddSubscriptionsToExistingGroup").show();


    $('.addSubscriptionParent').hide();
    if ($('#GridFilterdSubscriptions .e-row').length > 0 && SelectedSubscription.length > 0) {
        $('.addSubscriptionParent').show();
        $('#modal-SubscriptionDetails').css("display", "none");
    }

    if (visibleButton == "idAddSubscriptionsToGroup" && ej.base.getInstance(document.getElementById("querybuilder"), ej.querybuilder.QueryBuilder) == null) {
        bindqueryBuilder();
    }
}

function bindqueryBuilder() {

    var filter = getQueryFilterMain()
    var importRules = {
        'condition': 'and',
        'rules': [{
            'label': 'Category',
            'field': 'Category',
            'type': 'string',
            'operator': 'in',
            'value': ['Clothing']
        },
        {
            'condition': 'or',
            'rules': [{
                'label': 'TransactionType',
                'field': 'TransactionType',
                'type': 'string',
                'operator': 'equal',
                'value': 'Income'
            },
            {
                'label': 'PaymentMode',
                'field': 'PaymentMode',
                'type': 'string',
                'operator': 'equal',
                'value': 'Cash'
            }
            ]
        }
        ]
    };
    var qryBldrObj = new ej.querybuilder.QueryBuilder({
        //dataSource: window.expenseData,
        columns: filter,
        width: '100%',
        rule: {},
        ruleChange: updateRule
    });
    qryBldrObj.appendTo('#querybuilder');

    function updateRule(args) {
        //console.log(args.rule);
    }

    function getQueryFilterForLocation() {
        return filter = {
            field: 'Location',
            label: 'Location',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classLocationValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: locationData,
                        // maps the appropriate column to fields property
                        fields: { text: 'Name', value: 'Id' },
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.Id, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }

    function getQueryFilterForShift() {
        return filter = {
            field: 'Shift',
            label: 'Shift',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classShiftValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: shiftData,
                        fields: { text: 'Name', value: 'Id' },
                        //value: args.values ? args.values : shiftData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.Id, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForDepartment() {
        return filter = {
            field: 'Department',
            label: 'Department',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classDepartmentValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: departmentData,
                        fields: { text: 'Name', value: 'Id' },
                        //value: args.values ? args.values : departmentData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.Id, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForCity() {
        return filter = {
            field: 'City',
            label: 'City',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classCityValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: cityData,
                        fields: { text: 'Name', value: 'Id' },
                        //value: args.values ? args.values : cityData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.Id, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForEmail() {
        return filter = {
            field: 'Email',
            label: 'Email',
            type: 'string',
            operators: getOperatorsForDataField("string"),
        };
    }

    function getQueryFilterForName() {
        return filter = {
            field: 'Name',
            label: 'Name',
            type: 'string',
            operators: getOperatorsForDataField("Name"),
        };
    }

    function getQueryFilterForUpn() {
        return filter = {
            field: 'Upn',
            label: 'Upn',
            type: 'string',
            operators: getOperatorsForDataField("string"),
        };
    }
    function getQueryFilterForState() {
        return filter = {
            field: 'State',
            label: 'State',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classStateValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: stateData,
                        fields: { text: 'Name', value: 'Id' },
                        //value: args.values ? args.values : stateData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.Id, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForCountry() {
        return filter = {
            field: 'Country',
            label: 'Country',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classCountryValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: countryData,
                        fields: { text: 'Name', value: 'Id' },
                        //value: args.values ? args.values : countryData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.Id, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForJobTitle() {
        return filter = {
            field: 'JobTitle',
            label: 'JobTitle',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classJobTitleValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: jobTitleData,
                        fields: { text: 'Name', value: 'Name' },
                        //value: args.values ? args.values : countryData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.value, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForEmployeeType() {
        return filter = {
            field: 'EmployeeType',
            label: 'EmployeeType',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classEmployeeTypeValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: employeeTypeData,
                        fields: { text: 'Name', value: 'Name' },
                        //value: args.values ? args.values : countryData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.value, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForSubscribeOn() {
        return filter = {
            field: 'SubscribedOn',
            label: 'SubscribedOn',
            type: 'date',
            format: 'MMM dd, yyyy',
            operators: getOperatorsForDataField("DateTime"),
        }
    }
    function getQueryFilterForEmployeeGroup() {
        return filter = {
            field: 'EmployeeGroup',
            label: 'EmployeeGroup',
            type: 'string',
            operators: getOperatorsForDataField("integer"),
            template: {
                create: function () {
                    elem = document.createElement('input');
                    elem.setAttribute('type', 'text');
                    elem.setAttribute('class', 'classEmployeeGroupValue');
                    return elem;
                },
                destroy: function (args) {
                    var dropdown = ej.base.getComponent(document.getElementById(args.elementId), 'dropdownlist');
                    if (dropdown) {
                        dropdown.destroy();
                    }
                },
                write: function (args) {
                    dropDownObj = new ej.dropdowns.DropDownList({
                        dataSource: employeeGroupData,
                        fields: { text: 'Name', value: 'Name' },
                        //value: args.values ? args.values : countryData[0].Id,
                        change: function (e) {
                            qryBldrObj.notifyChange(e.itemData.value, e.element);
                        }
                    });
                    dropDownObj.appendTo('#' + args.elements.id);
                }
            }
        }
    }
    function getQueryFilterForCostCenter() {
        return filter = {
            field: 'CostCenter',
            label: 'CostCenter',
            type: 'string',
            operators: getOperatorsForDataField("string"),
        };
    }
    function getQueryFilterMain() {
        let mainFilter = [];
        mainFilter.push(getQueryFilterForLocation());
        mainFilter.push(getQueryFilterForShift());
        mainFilter.push(getQueryFilterForDepartment());
        mainFilter.push(getQueryFilterForEmail());
        mainFilter.push(getQueryFilterForName());
        mainFilter.push(getQueryFilterForCity());
        mainFilter.push(getQueryFilterForUpn());
        mainFilter.push(getQueryFilterForState());
        mainFilter.push(getQueryFilterForCountry());
        mainFilter.push(getQueryFilterForJobTitle());
        mainFilter.push(getQueryFilterForEmployeeType());
        mainFilter.push(getQueryFilterForCostCenter());
        mainFilter.push(getQueryFilterForEmployeeGroup());
        mainFilter.push(getQueryFilterForSubscribeOn());
        return mainFilter;
    }

}

function getOperatorsForDataField(type) {
    if (type == "integer") {
        return operator = [
            { key: 'Equal', value: 'equal' },
            { key: 'Not Equal', value: 'notequal' },
            { key: 'Is Null', value: 'isnull' },
            { key: 'Is Not Null', value: 'isnotnull' }
        ];
    }
    else if (type == "string") {
        return operator = [
            { key: 'Starts With', value: 'startswith' },
            { key: 'Ends With', value: 'endswith' },
            { key: 'Contains', value: 'contains' },
            { key: 'Equal', value: 'equal' },
            { key: 'Not Equal', value: 'notequal' },
            { key: 'Is Null', value: 'isnull' },
            { key: 'Is Not Null', value: 'isnotnull' }
        ];
    }
    else if (type == "Name") {
        return operator = [
            { key: 'Contains', value: 'contains' },
            { key: 'Equal', value: 'equal' },
            { key: 'Not Equal', value: 'notequal' },
            { key: 'Is Null', value: 'isnull' },
            { key: 'Is Not Null', value: 'isnotnull' }
        ];
    }
    else if (type == "DateTime") {
        return operator = [
            { key: 'Equal', value: 'equal' },
            { key: 'Not Equal', value: 'notequal' },
            { key: 'Greater Than', value: 'greaterthan' },
            { key: 'Less Than', value: 'lessthan' },
            { key: 'Greater Than Or Equal', value: 'greaterthanorequal' },
            { key: 'Less Than Or Equal', value: 'lessthanorequal' },
        ];
    }
}

function setFilteredCount() {
 
    var grid = document.getElementById("GridFilterdSubscriptions").ej2_instances[0];
    let count = 0;
    let selectedPeople = SelectedSubscription.length;
    if (grid != undefined && grid.pageSettings != undefined && grid.pageSettings.totalRecordsCount > 0)
        count = grid.pageSettings.totalRecordsCount;
     
    let subscriptionCount = "<b>" + selectedPeople + "/" + count + "</b>" + " People Selected";

    $('#idMainMenuSelectedSubscriptions').html("<i class='fas fa-user-friends' aria-hidden='true'></i> " + subscriptionCount);
}

function filteredRowSelected(event) {
    if (event.target == undefined || event.target == null)
        return false;

    if (event.isHeaderCheckboxClicked) //selectALl
    {
        SelectedSubscription = [];
        SelectedSubscription = event.data.map((d) => d.Id);
    }
    else {
        let selectedId = "";
        if (event.data == undefined)
            selectedId = $(event.row).find(".customcssId").text();
        else
            selectedId = event.data.Id;
        if (selectedId != "" && selectedId != undefined) {
            if (SelectedSubscription.indexOf(event.data.Id) == -1)
                SelectedSubscription.push(event.data.Id);
        }
    }
    setFilteredCount();
    $('.addSubscriptionParent').show();
    if (SelectedSubscription.length == 0)
        $('.addSubscriptionParent').hide();

}

function filteredRowDeSelected(event) {
    if (event.target == undefined || event.target == null)
        return false;

    if (event.isHeaderCheckboxClicked) //selectALl
    {
        SelectedSubscription = [];
    }
    else {
        let selectedId = "";
        if (event.data == undefined)
            selectedId = $(event.row).find(".customcssId").text();
        else
            selectedId = event.data.Id;
        if (selectedId != "" && selectedId != undefined) {
            const index = SelectedSubscription.indexOf(selectedId);
            if (index > -1 && SelectedSubscription.length > 0) {
                SelectedSubscription.splice(index, 1);
            }
        }
    }
    setFilteredCount();
    $('.addSubscriptionParent').hide();
    if (SelectedSubscription.length > 0)
        $('.addSubscriptionParent').show();
}

function navigatedToPages(event) {
    hideLoader();
}

function enableDisablePrivacyCheckbox() {
    $("#idPrivacyCheckbox").removeClass("disableField");
    if (!$("#idAccessOnlyToOwner").prop("checked")) {
        $("#idPrivacyCheckbox").addClass("disableField");
        $("#idAccessToMembers").prop("checked", false);
    }
}

function enableDisableIsAdminCheckbox() {
    $("#idIsAccessToAdminsCheckbox").removeClass("disableField");
    if (!$("#IsPrivateEnabled").prop("checked")) {
        $("#idIsAccessToAdminsCheckbox").addClass("disableField");
        $("#IsAdminAccessEnabled").prop("checked", false);
    }
}

$("#IsPrivateEnabled").change(function () {
    enableDisableIsAdminCheckbox();
    if ($(this).prop('checked')) {
        $(this).attr('checked', true);
    } else {
        $(this).attr('checked', false);
    }
});

$('#IsAdminAccessEnabled').change(function (event) {
    if ($(this).prop('checked')) {
        $(this).attr('checked', true);
    } else {
        $(this).attr('checked', false);
    }
});

function deleteGroups(event) {
    let id = $(event).attr("id");
    let name = $(event).attr("groupname");
    $("#idDeleteGroupText").text("Are you sure you want to delete " + name + " ?");
    $("#idGroupIdForDelete").val(id);
}

function getSubscriptionEmail(event) {
    let officialemail = $(event).attr("name");
    $("#idPopupContent").text("Are you sure you want to delete " + officialemail + " ?");
    $("#hdnSubscriptionId").val(officialemail);
}

function restoreGroup(event) {
    let id = $(event).attr("id");
    let name = $(event).attr("groupname");
    $("#idRestoreGroupText").text("Please click Confirm to restore the Group " + name + " and its members.");
    $("#idGroupIdForRestore").val(id);
}

