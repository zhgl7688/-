fruit.databind.default.binder.cascadeFilter = function (databind, target, options) {
	var db = databind;
	var $element = $(target);
	var opt = options.cascadeFilter;

	var parentValueName = 'form.' + opt.ParentField;
	var dataIsLoaded = false;

	var parentCombobox = db.jq.find('.easyui-combobox[data-bind*="value:\'' + parentValueName + '\'"]');

	function getParentValue() {
		return parentCombobox.combobox('getValue');
	}

	function updateSources() {
		var newValue = getParentValue();
		var isNull = typeof (newValue) == 'undefined' || newValue == null || newValue == '';
		var comboboxData = $element.data('combobox');
		if (!dataIsLoaded && comboboxData.data.length > 0) {
			$element.data('combobox').allData = $element.data('combobox').data;
			dataIsLoaded = true;
		}
		var allData = $element.data('combobox').allData;
		if ($.isArray(allData)) {
			var newData = [];
			if (isNull) {
				if (opt.FullIfParentNull) {
					newData = allData;
				}
			} else {
				$.each(allData, function () {
					if (this[opt.ConditionField] == newValue) {
						newData.push(this);
					}
				});
			}
			$element.combobox('loadData', newData);
			if (newData.length) {
				$element.combobox('setValue', newData[0].value);
			}
		}
	}

	$element.combobox({
		onLoadSuccess: function () {
			dataIsLoaded = true;
			$element.data('combobox').allData = $element.data('combobox').data;
		}
	});

	if (parentCombobox && parentCombobox.length) {
		parentCombobox.combobox({
			onChange: function (newValue, oldValue) {
				updateSources(newValue);
			}
		})
	}
}