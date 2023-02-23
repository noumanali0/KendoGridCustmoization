var SR = SR || {}




SR.DprPro = function () {
    var self = this;
    var url = '@Url.Content("~Create/Home")';


    


    $(window).on("load", function () {
        
        
       
    });


    self.rowTemplate = function(data)  {
        return `<div class="rowTemp"><i class="fa fa-caret-right p-2"></i> <p>${data.StudentID}</p></div>`;
    }

    self.error_handler = function(e) {
        if (e.errors) {
            var message = "Errors:\n";
            $.each(e.errors, function (key, value) {
                if ('errors' in value) {
                    $.each(value.errors, function () {
                        message += this + "\n";
                    });
                }
            });
            alert(message);
        }
    }


    self.onChange = function (e) {
        window.location.href = "Home/Home"
    }


    


    self.onDropDownChange = function (e) {
        var orders = $("#acitvity").data("kendoDropDownList");
        orders.value("");

        if (e.sender.value() == "") {
            orders.enable(false);
        }
        else {
            orders.enable(true);
        }
    }


    self.initializeDialog = function () {
        $("#dialog").kendoDialog({
            width: "450px",
            height: "300px",
            title: "New DPR",
            content: `<div class="d-block">
                
                <div class="block">
                    <label for="datepicker" class="pickerLabel">Date of Works</label>
                    <div class="datePicker">
                        <input id='datepicker' value="99/99/9999"/>
                    </div>
                </div>
                
                <div class="block">
                    <label for="transport" class="pickerLabel">Transport</label>
                    <div class="transport">
                        <input id="transportdropdown"/>
                    </div>
                </div>
                
                <div class="block">
                    <label for="template" class="pickerLabel">Template</label>
                    <div class="template">
                        <input id="templatedropdown"/>
                    </div>
                </div>

                </div>`,
            actions: [

                {
                    text: '',
                    action: function (e) {
                        $.ajax({
                            url: '/Home/Called',
                            type: 'GET',
                            success: function (result) {
                                window.location.href = "/Home/Create";
                                $("dialog").close()
                                ///// what ation will be performed on dialog button click 
                            }
                        });
                    }
                },
                {
                    text: '',
                    action: function (e) {
                        ///// what ation will be performed on dialog button click 
                    }
                }
            ]
        });
    }



    self.toggleDrawer = function () {
        var drawerInstance = $("#drawer").data().kendoDrawer;
        var drawerContainer = drawerInstance.drawerContainer;
        if (drawerContainer.hasClass("k-drawer-expanded")) {
            drawerInstance.hide();
        } else {
            drawerInstance.show();
        }
    }

    self.onItemClick = function(e) {
        if (!e.item.hasClass("k-drawer-separator")) {
            e.sender.drawerContainer.find("#drawer-content > div").addClass("hidden");
            e.sender.drawerContainer.find("#drawer-content").find("#" + e.item.find(".k-item-text").attr("data-id")).removeClass("hidden");
        }
    }

    self.openDialog = function () {

        self.initializeDialog()
        $("#dialog").data("kendoDialog").open();

        $("#datepicker").kendoDatePicker()

        $("#transportdropdown").kendoDropDownList({
            dataSource: ["Option 1", "Option 2", "Option 3"],
            optionLabel: "Transport Name"
        });


        $("#templatedropdown").kendoDropDownList({
            dataSource: ["Option 1", "Option 2", "Option 3"],
            optionLabel: "Template"
        });

        $(".k-hstack button:first-child").append(`<span class="glyphicon glyphicon-floppy-disk"></span>`)


        $(".k-hstack button:nth-child(2)").append(`<span class="fa fa-ban"></span>`)
    }

}


if (SR.DprPro.Instance == null) {
    SR.DprPro.Instance = new SR.DprPro();
}






