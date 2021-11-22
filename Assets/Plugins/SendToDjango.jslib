mergeInto(LibraryManager.library,
{
    AlertParam: function(param){
        window.alert(Pointer_stringify(param));
    },

    PostLevel: function(level){
        GetLevel(level);
    },
});