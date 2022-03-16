import './App.css';
import React, { useState } from "react";
import { PageLayout } from "../src/components/PageLayout";
import { AuthenticatedTemplate, UnauthenticatedTemplate, useMsal } from "@azure/msal-react";
import { loginRequest } from "./authConfig";
import Button from "react-bootstrap/Button";
import Profile from './Profile';
import { ProfileData } from "./components/ProfileData";
import { callMsGraph } from "./graph";

function App() {
  return (
      <PageLayout>
          <AuthenticatedTemplate>
              <ProfileContent />
              <Profile/>
          </AuthenticatedTemplate>
          <UnauthenticatedTemplate>
            <center><p>You are not signed in! Please sign in.</p></center>
          </UnauthenticatedTemplate>
      </PageLayout>
  );
}

function ProfileContent() {
  const { instance, accounts } = useMsal();
  const [graphData, setGraphData] = useState(null);

  const name = accounts[0] && accounts[0].name;

  function RequestProfileData() {
      const request = {
          ...loginRequest,
          account: accounts[0]
      };

      // Silently acquires an access token which is then attached to a request for Microsoft Graph data
      instance.acquireTokenSilent(request).then((response) => {
          callMsGraph(response.accessToken).then(response => setGraphData(response));
      }).catch((e) => {
          instance.acquireTokenPopup(request).then((response) => {
              callMsGraph(response.accessToken).then(response => setGraphData(response));
          });
      });
  }

  return (
      <>
          <center>
          <h5 className="card-title">Welcome {name}</h5>
          {graphData ? 
              <ProfileData graphData={graphData} />
              :
              <Button variant="secondary" onClick={RequestProfileData}>Request Profile Information</Button>
          }
          </center>
      </>
  );
};

export default App;
