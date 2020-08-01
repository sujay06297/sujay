(function (global, $) {

  var Greetr = function (firstname, lastname, language) {
    return new Greetr.init(firstname, lastname, language);
  }

  var supportedLangs = ['en', 'zh_tw'];

  var greetings = {
    en: 'Hello',
    zh_tw: '你好'
  };

  var formalGreetings = {
    en: 'Greetings',
    zh_tw: '歡迎您'
  };

  var logMessages = {
    en: 'Logged in',
    zh_tw: '登入'
  };

  Greetr.prototype = {

    fullName: function () {
      return this.firstname + ' ' + this.lastname;
    },

    validate: function () {
      if (supportedLangs.indexOf(this.language) === -1) {
        throw "Invalid language";
      }
    },

    greeting: function () {
      return greetings[this.language] + ' ' + this.firstname + '!';
    },

    formalGreeting: function () {
      return formalGreetings[this.language] + ' ' + this.fullName();
    },

    greet: function (formal) {
      var msg;
      //  if undefined or null, it will be coerced to 'false'
      if (formal) {
        msg = this.formalGreeting();
      } else {
        msg = this.greeting();
      };
      if (console) {
        console.log(msg);
      };
      // 'this' refers to the calling object at execution time
      // makes the method chainable
      return this;
    },

    greetHTML: function (selector, formal) {
      if (!$) throw "jQuery not loaded";
      if (!selector) throw "no seletor";
      let msg = "";
      if (formal) {
        msg = this.formalGreeting();
      } else {
        msg = this.greeting();
      };
      $(selector).html(msg);
      return this;
    },

    log: function () {
      if (console) {
        console.log(logMessages[this.language] + ' ' + this.fullName());
      }
      return this;
    },

    setLang: function (lang) {
      this.language = lang;
      this.validate();
      return this;
    }

  }

  Greetr.init = function (firstname, lastname, language) {
    var self = this;
    self.firstname = firstname || '';
    self.lastname = lastname || '';
    self.language = language || 'zh_tw';
  }

  Greetr.init.prototype = Greetr.prototype;
  global.Greetr = global.G$ = Greetr;
})(window, jQuery)