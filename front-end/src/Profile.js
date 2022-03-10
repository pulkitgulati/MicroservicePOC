import './App.css';
import { useEffect, useState } from "react";
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import axios from 'axios';
import MaterialTable from "material-table";

import { forwardRef } from 'react';
import AddBox from '@material-ui/icons/AddBox';
import ArrowDownward from '@material-ui/icons/ArrowDownward';
import Check from '@material-ui/icons/Check';
import ChevronLeft from '@material-ui/icons/ChevronLeft';
import ChevronRight from '@material-ui/icons/ChevronRight';
import Clear from '@material-ui/icons/Clear';
import DeleteOutline from '@material-ui/icons/DeleteOutline';
import Edit from '@material-ui/icons/Edit';
import FilterList from '@material-ui/icons/FilterList';
import FirstPage from '@material-ui/icons/FirstPage';
import LastPage from '@material-ui/icons/LastPage';
import Remove from '@material-ui/icons/Remove';
import SaveAlt from '@material-ui/icons/SaveAlt';
import Search from '@material-ui/icons/Search';
import ViewColumn from '@material-ui/icons/ViewColumn';

const tableIcons = {
    Add: forwardRef((props, ref) => <AddBox {...props} ref={ref} />),
    Check: forwardRef((props, ref) => <Check {...props} ref={ref} />),
    Clear: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
    Delete: forwardRef((props, ref) => <DeleteOutline {...props} ref={ref} />),
    DetailPanel: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
    Edit: forwardRef((props, ref) => <Edit {...props} ref={ref} />),
    Export: forwardRef((props, ref) => <SaveAlt {...props} ref={ref} />),
    Filter: forwardRef((props, ref) => <FilterList {...props} ref={ref} />),
    FirstPage: forwardRef((props, ref) => <FirstPage {...props} ref={ref} />),
    LastPage: forwardRef((props, ref) => <LastPage {...props} ref={ref} />),
    NextPage: forwardRef((props, ref) => <ChevronRight {...props} ref={ref} />),
    PreviousPage: forwardRef((props, ref) => <ChevronLeft {...props} ref={ref} />),
    ResetSearch: forwardRef((props, ref) => <Clear {...props} ref={ref} />),
    Search: forwardRef((props, ref) => <Search {...props} ref={ref} />),
    SortArrow: forwardRef((props, ref) => <ArrowDownward {...props} ref={ref} />),
    ThirdStateCheck: forwardRef((props, ref) => <Remove {...props} ref={ref} />),
    ViewColumn: forwardRef((props, ref) => <ViewColumn {...props} ref={ref} />)
  };

  const columns = [
    { title: "PersonID", field: "personID", defaultSort: "desc" },
    { title: "FirstName", field: "firstName", defaultSort: "desc" },
    { title: "MiddleName", field: "middleName", defaultSort: "desc" },
    { title: "LastName", field: "lastName", defaultSort: "desc" },
    { title: "Email", field: "email", defaultSort: "desc" },
  ];

function Profile() {
  const [inputs, setInputs] = useState({});
  const [textarea, setTextarea] = useState(
    ""
  );
  const [message, setMessage] = useState("");
  const [persons, setPersons] = useState([{personID: 1, firstName: "Pulkit", middleName: "", lastName: "Gulati", email: "test@test.com"}]);

  const getData = () => {
    axios({
      method: 'post',
      url: 'https://<add your domain>.azurewebsites.net/Authentication/Token',
      headers: {
        'Cookie': 'ARRAffinity=0f2ba6f32722b5372246745155b9aa8f1968d09c71e7a67cc711750fef8a3f91; ARRAffinitySameSite=0f2ba6f32722b5372246745155b9aa8f1968d09c71e7a67cc711750fef8a3f91',
        'Content-Type': 'application/json',
        },
      data: {
        "username": "demo",
        "password": "xxxx"
      }
    }).then(auth=>{
      console.log(auth.data);
      axios({
        method: 'get',
        url: 'https://<add your domain>.azure-api.net/Name/getallname',
        headers: {
        'Authorization': `bearer ${auth.data.token}`,
        'Content-Type': 'application/json',
        }, 
      }).then(data2=> {
        //fetch addresses
        axios({
          method: 'get',
          url: 'https://<add your domain>.azure-api.net/addr/Address/GetAllAddresses',
          headers: {
          //'Authorization': `bearer ${auth.data.token}`,
          'Content-Type': 'application/json',
          }, 
        }).then(addresses=> {
          var addr = addresses.data;
          var names = data2.data;
          for(var i = 0; i < addr.length; i++) {
            for(var j=0; j < names.length; j++) {
              if((names[j].personID == addr[i].personID) && !names[j].hasOwnProperty('email')) {
                names[j].email = addr[i].email;
                break;
              }
            }
          }
          setPersons(names);
          console.log(names);
        });
      });
  });

  }

  useEffect(() => {
    getData();
  }, [])

  const handleChange = (event) => {
    const name = event.target.name;
    const value = event.target.value;
    setInputs(values => ({...values, [name]: value}))
  }

  const handleTextChange = (event) => {
    setTextarea(event.target.value);
  }

  const handleSubmit = (event) => {
    event.preventDefault();
    console.log(inputs, textarea);
    const payload = {
      firstname: inputs.firstname,
      lastName: inputs.lastname,
      middleName: inputs.middlename,
      address: textarea,
      phone: inputs.phone,
      email: inputs.email
    };
    console.log(payload);
    axios({
      method: 'post',
      url: 'https://<add your domain>.azure-api.net/Name/CreateProfile',
      data: payload, // you are sending body instead
      headers: {
       // 'Authorization': `bearer ${token}`,
      'Content-Type': 'application/json',
      //'Ocp-Apim-Subscription-Key': 'd7c7d3e87f034bb794a345fd9d56c8f4'
      //'Ocp-Apim-Trace': 'true'
      }, 
    })
    .then(data=> {
      console.log(data);      
      setMessage(data.data);
      getData();
    });
  }

  return (
    <div class="container" style={{backgroundColor: 'gray', width: 800, marginTop: 10, paddingTop: 10}}>
      <div class="">
        <MaterialTable
          title={"People Details"}
          icons={tableIcons}
          columns={columns}
          data={persons}
          options={{
            sorting: true
          }}
        />
      </div>
      <form onSubmit={handleSubmit}>
        <div class="form-group">
          <label>Enter your First Name:
          <input
            type="text" 
            name="firstname" 
            value={inputs.firstname || ""}
            class="form-control" 
            onChange={handleChange}
          />
          </label>
        </div>
        <br />
        <div class="form-group">
          <label>Enter your Middle Name:
          <input
            type="text" 
            name="middlename"
            class="form-control" 
            value={inputs.middlename || ""} 
            onChange={handleChange}
          />
          </label>
        </div>
        <br />
        <div class="form-group">
          <label>Enter your Last Name:
          <input
            type="text" 
            name="lastname"
            class="form-control" 
            value={inputs.lastname || ""} 
            onChange={handleChange}
          />
          </label>
        </div>
        <br />
        <div class="form-group">
          <label>Enter your Email:
          <input
            type="text" 
            name="email"
            class="form-control" 
            value={inputs.email || ""} 
            onChange={handleChange}
          />
          </label>
        </div>
        <br />
        <div class="form-group">
          <label>Enter your Phone Number:
          <input
            type="text" 
            name="phone" 
            value={inputs.phone || ""}
            class="form-control" 
            onChange={handleChange}
          />
          </label>
        </div>
        <br />
        <div class="form-group">
          <label>Enter your Address:
          <textarea name="address" value={textarea} onChange={handleTextChange} class="form-control" />
          </label>
        </div>
        <input type="submit" class="btn btn-primary" />
        <br /><br /><br />
        <div class="form-group">
          <label type="label" className='alert alert-info'>
            {message}
          </label>
        </div>
      </form>
    </div>
  )
}

export default Profile;
