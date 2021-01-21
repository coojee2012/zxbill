var answered = false;
var trys = 0;
while (trys < 60) {
  var uuid = session.uuid;
  console_log("info", "uuid:" + uuid);
  var direction = session.getVariable("direction");
  console_log("info", "direction:" + direction);
  var state = session.state;
  console_log("info", "state:" + state);
  var originating_leg_uuid = session.getVariable("originating_leg_uuid");
  console_log("info", "originating_leg_uuid:" + originating_leg_uuid);

  var variable_call_uuid = session.getVariable("call_uuid");
  console_log("info", "variable_call_uuid:" + variable_call_uuid);

  var keys = Object.keys(session);
  console_log("info", "keys:" + keys.join(","));
  session.sleep(1000);
  trys++;
}
