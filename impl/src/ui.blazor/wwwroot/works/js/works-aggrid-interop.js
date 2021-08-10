// This file is to show how a library package may provide JavaScript interop features
// wrapped in a .NET API

window.getVisaoConfigure = function (gridOptions) {
    gridOptions.getDataPath = function (data) {
        var dataTree = data.Componente;
        var dataArray = dataTree.split(",");
        return dataArray;
    }
};

window.example4_fetchData5_albums_configure = function (gridOptions) {
    gridOptions.getRowNodeId = function (node) {
        return "" + node.id;
    }
};
window.example4_fetchData5_photos_configure = function (gridOptions) {
    gridOptions.getRowNodeId = function (node) {
        return "ID#" + node.id;
    }
};