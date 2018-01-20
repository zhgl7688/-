fruit.databind.default.binder.orgtree = function (databind, target, options) {
    var b = this;
    databind.tree = this;
    this.db = databind;
    this.jq = $(target);
    this.options = fruit.parseOptions(options, target, databind.options, { nodeDblClick: $.noop }, 'url', 'idField', 'parentField', 'treeField', 'nodeDblClick', 'checkbox');

    if (this.options.checkbox) {
        this.jq.addClass('org-tree-checkbox');
        this.options.selectedNodes = [];
    }

    this.loadData = function (data) {

        b.options.data = data;

        function buildTreeNode(data, level, parentElement, parentNodeId) {
            var rootTbl, rootTrs;
            var showCount = 0;
            var startTrIdx = level > 0 ? 2 : 0;
            for (var i = 0; i < data.length; i++) {
                var row = data[i];
                if (row[b.options.parentField] == parentNodeId) {
                    showCount++;

                    rootTbl = $('<table cellspacing=0 cellpadding=0 align="center"><tr></tr><tr></tr><tr></tr></table>');

                    if (level > 0) {
                        $('<tr>').appendTo(rootTbl);
                        $('<tr>').appendTo(rootTbl);
                    }

                    rootTrs = rootTbl.find('tr');

                    if (level > 0) {
                        if (showCount == 1) {
                            $('<td><table cellspacing=0 cellpadding=0 align="center" width="100%"><tr><td style="height:3px"></td><td style="height:3px;background-color:#000;"></td></tr></table></td>').appendTo(rootTrs[0]);
                        } else {
                            $('<td style="height:3px;background-color:#000;"></td>').appendTo(rootTrs[0]);
                        }
                        $('<td class="orgtree-vline"></td>').appendTo(rootTrs[1]);
                    }

                    var nodeEle = $('<table class="orgtree-node" cellspacing="0" cellpadding="0" align="center"><tr><td align="center">' + row[b.options.treeField] + '</td></tr></table>').attr('node-id', row[b.options.idField]);
                    var nodeCell = $('<td valign="top" align="center"></td>');
                    nodeCell.append(nodeEle).appendTo(rootTrs[startTrIdx]);
                    $('<td class="orgtree-vline"></td>').appendTo(rootTrs[startTrIdx + 1]);
                    var childTbl = $('<td valign="top"><table cellspacing=0 cellpadding=0 align="center"><tr></tr></table></td>').appendTo(rootTrs[startTrIdx + 2]);

                    if (level == 0) {
                        rootTbl.appendTo(parentElement);
                    } else {
                        $('<td valign="top"></td>').appendTo(parentElement).append(rootTbl);
                    }

                    var childCount = buildTreeNode(data, level + 1, childTbl.find('tr')[0], row[b.options.idField]);
                    if (level > 0) {
                        if (childCount == 0) {
                            var ec = rootTrs.length;
                            $(rootTrs[ec - 1]).remove();
                            $(rootTrs[ec - 2]).remove();
                        } else if (childCount == 1) {
                            //$(rootTrs[0]).find('td:last').css('backgroundColor', 'red');
                        }
                    }
                }
            }

            if (level > 0) {
                if (showCount == 1) {
                    $(rootTrs[1]).remove();
                    $(rootTrs[0]).remove();
                } else if (showCount > 1) {
                    $(rootTrs[0]).html('<td><table cellspacing=0 cellpadding=0 align="center" width="100%"><tr><td style="height:3px;background-color:#000;"></td><td style="height:3px;"></td></tr></table></td>');
                }
            }
            return showCount;
        }

        this.jq.empty();
        buildTreeNode(data, 0, this.jq, '0');

    }

    this.getData = function () {
        return b.options.data;
    }

    this.getTreeData = function (rowHandler) {
        if (!$.isFunction(rowHandler)) {
            rowHandler = function (row) {
                row.id = row[b.options.idField];
                row.text = row[b.options.treeField];
            }
        }
        var rows = b.options.data;
        var filter = fruit.tree.createLoadFilter(b.options.idField, b.options.parentField, '0', rowHandler)
        return filter(rows);
    }

    this.select = function (id) {
        if (this.options.checkbox) {
            var hasSelected = this.options.selectedNodes.indexOf(function (i) { return i == id });
            if (hasSelected > -1) {
                this.options.selectedNodes.splice(hasSelected, 1);
                this.jq.find('.orgtree-node[node-id="' + id + '"]').removeClass('orgtree-node-selected');
            } else {
                this.options.selectedNodes.push(id);
                this.jq.find('.orgtree-node[node-id="' + id + '"]').addClass('orgtree-node-selected');
            }
        } else {
            this.jq.find('.orgtree-node').removeClass('orgtree-node-selected');
            b.options.selectedNode = b.options.data.find(b.options.idField, id);
            this.jq.find('.orgtree-node[node-id="' + id + '"]').addClass('orgtree-node-selected');
        }
    }

    this.getSelected = function () {
        return b.options.selectedNode;
    }

    this.getSelecteds = function () {
        return b.options.selectedNodes;
    }

    this.setSelecteds = function (ids) {
        if ($.isString(ids)) {
            ids = ids.splice(',');
        }
        this.options.selectedNodes = ids;
        this.jq.find('.orgtree-node').removeClass('orgtree-node-selected');
        $.each(ids, function () {
            b.jq.find('.orgtree-node[node-id="' + this + '"]').addClass('orgtree-node-selected');
        });
    }

    this.delete = function () {
        if (b.options.selectedNode) {
            b.options.data.remove(b.options.selectedNode);
            b.options.selectedNode = null;

            b.loadData(b.options.data);
        }
    }
    
    this.refresh = function () {
        if (b.options.url) {
            b.jq.showLoading();
            fruit.ajax({
                method: 'get',
                url: b.options.url,
                success: function (data) {
                    b.jq.hideLoading();
                    b.loadData(data);
                }
            })
        }
    }

    this.jq.on('click', '.orgtree-node', function () {
        b.select.call(b, $.attr(this, 'node-id'));
    });
    this.jq.on('dblclick', '.orgtree-node', function () {
        var id = $.attr(this, 'node-id');
        if ($.isFunction(b.options.nodeDblClick)) {
            b.options.nodeDblClick.call(b, id);
        } else if ($.isString(b.options.nodeDblClick)) {
            b.db.invoke(b.options.nodeDblClick, id);
        }
        //b.select($.attr(this, 'node-id'));
    })
    
    this.refresh();
}