export const UserSessionValidation = async (): Promise<boolean> => {
  try {
    const response = await fetch('/api/Accounts/authenticated', {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include'
    });
    if (!response.ok) {
      if (response.status === 401) {
        const errorData = await response.json();
        const errorMesssage = errorData.message || 'Unauthorized';
        throw new Error(errorMesssage);
      }
      return false;
    }
    return true;

  }
  catch (e) {
    if (e instanceof Error) {
      console.error("Login Check Error", e);
      alert(`Error checking login status ${e.message}`);
    }

    return false;
  }
}
export const GetResponseServerAPI = async (url: string): Promise<Response> => {
  try {
    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include'
    });
    if (!response.ok) {
      let errorMessage = `HTTP error! status: ${response.status}`;
      let errorData;
      try {
        errorData = await response.json();
        if (errorData && errorData.message) {
          errorMessage += ` - ${errorData.message}`;
        }
      } catch (jsonError) {
        // console.error("Error parsing JSON:", jsonError);
      }
      // console.error(errorMessage);
      throw new Error(errorMessage);
    }

    return response;
  }
  catch (error) {
    if (error instanceof TypeError && error.message === "Failed to fetch") {
      throw new Error("Network error. Please check your connection.");
    }
    throw error;
  }
}

export const PostResponseServerAPI = async (url: string, data: any, options?: RequestInit): Promise<Response> => {
  try {
    const response = await fetch(url, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      credentials: 'include',
      body: JSON.stringify(data)
    });
    if (!response.ok) {
      let errorMessage = `HTTP error! status: ${response.status}`;
      let errorData;

      try {
        errorData = await response.json();
        if (errorData && errorData.message) {
          errorMessage += ` - ${errorData.message}`;
        }
      } catch (jsonError) {
      }
      throw new Error(errorMessage);
    }

    return response;
  } catch (error) {
    if (error instanceof TypeError && error.message === "Failed to fetch") {
      throw new Error("Network error. Please check your connection.");
    }
    throw error;
  }
}
