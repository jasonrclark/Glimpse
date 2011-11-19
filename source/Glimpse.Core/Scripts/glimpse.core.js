﻿/*(import:jquery-1.6.3.min.js)*/

var glimpse = (function ($, scope) {
    var //Private
        elements = {},
        template = {},
        config = {
            path : '',
            popupUrl : 'test-popup.html'
        },
        settings = {
            height : 250,
            activeTab: 'Routes'
        },
/*(import:glimpse.core.util.js|2)*/,
/*(import:glimpse.core.objects.js|2)*/,
/*(import:glimpse.core.pubsub.js|2)*/,
/*(import:glimpse.core.state.js|2)*/,
/*(import:glimpse.core.data.js|2)*/,
/*(import:glimpse.core.process.js|2)*/, 
/*(import:glimpse.core.template.js|2)*/,
/*(import:glimpse.controller.render.js|2)*/,
/*(import:glimpse.controller.shell.js|2)*/,
/*(import:glimpse.controller.sizer.js|2)*/,
/*(import:glimpse.controller.tollbar.js|2)*/, 
/*(import:glimpse.controller.metadata.js|2)*/,
/*(import:glimpse.controller.title.js|2)*/, 
/*(import:glimpse.controller.lazyloader.js|2)*/, 
/*(import:glimpse.controller.notification.js|2)*/,
/*(import:glimpse.controller.paging.js|2)*/,
/*(import:glimpse.render.engine.js|2)*/, 
        init = function () {
            pubsub.publish('state.init'); 
            pubsub.publish('state.build');  
            pubsub.publish('state.render'); 
            pubsub.publish('state.final'); 
        };
        

    return { 
        init : init,
        pubsub : pubsub, 
        elements : elements,
        render : renderEngine,
        objects : objects,
        data : data,
        util : util,
        settings : settings
    };
}($Glimpse, $Glimpse(document)));

$Glimpse(document).ready(function() {
    var start = new Date().getTime();

    glimpse.init();

    var end = new Date().getTime(); 
    console.log('Total execution time: ' + (end - start));
});

/*(import:glimpse.plugin.ajax.js)*/
/*(import:glimpse.plugin.history.js)*/
/*(import:glimpse.plugin.timeline.js)*/
/*(import:google-code-prettify.js)*/