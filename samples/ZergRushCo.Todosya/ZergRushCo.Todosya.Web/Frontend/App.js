﻿/*
 * Main application module. Loads with every page load.
 */

define(['jquery'], function($) {
    var App = {};

    App.sayHello = function() {
        console.log('Hello');
    }

    App.contentTypeJson = 'application/json';

    return App;
});