tc.test("Código de Respuesta 200", function() {
  return tc.response.status == 200;
});
tc.test("Código de Respuesta 400", function() {
  return tc.response.status == 400;
});
tc.test("Código de Respuesta 404", function() {
  return tc.response.status == 404;
});
tc.test('Ciudad Actualizada', function() {
  return tc.response.json.ciudad == "Barranquilla"}
);
tc.test('Mensaje no encontrado', function(){
  return tc.response.text.includes("no encontrada");
});
tc.test("Id Elemnto OK", function() {
    let id = tc.response.json.id;
    let value = tc.getVar("EmpresaID");
    return id == value;
});
tc.test('Varios elementos', function() {  
    return tc.response.json.length > 0;
});

tc.setVar("EmpresaID", tc.response.json.id, "global");