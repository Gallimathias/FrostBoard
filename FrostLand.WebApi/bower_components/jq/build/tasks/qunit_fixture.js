var fs = require( "fs" );

module.exports = function( grunt ) {
	grunt.registerTask( "qunit_fixture", function() {
		var dest = "./test/data/qunit-fixture.js";
		fs.writeFileSync(
			dest,
			"// Generated by build/tasks/qunit_fixture.js\n" +
			"QUnit.config.fixture = " +
			JSON.stringify(
				fs.readFileSync(
					"./test/data/qunit-fixture.html",
					"utf8"
				).toString().replace( /\r\n/g, "\n" )
			) +
			";\n" +
			"// Compat with QUnit 1.x:\n" +
			"document.getElementById( \"qunit-fixture\" ).innerHTML = QUnit.config.fixture;\n"
		);
		grunt.log.ok( "Updated " + dest + "." );
	} );
};
